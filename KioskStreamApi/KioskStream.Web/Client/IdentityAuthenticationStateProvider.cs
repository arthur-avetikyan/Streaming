using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

using KioskStream.Web.Client.Services.Interfaces;
using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Common.DataTransferObjects.Account;

using Microsoft.AspNetCore.Components.Authorization;


namespace KioskStream.Web.Client
{
    public class IdentityAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenRefreshApiAccessor _tokenRefreshApiAccessor;
        private readonly ITokenStorage _tokenStorage;

        public IdentityAuthenticationStateProvider(ITokenRefreshApiAccessor tokenRefreshApiAccessor,
                                                   HttpClient httpClient,
                                                   ITokenStorage tokenStorage)
        {
            _tokenRefreshApiAccessor = tokenRefreshApiAccessor;
            _httpClient = httpClient;
            _tokenStorage = tokenStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = await GetAccessTokenAsync();
            ClaimsIdentity identity = new ClaimsIdentity();
            _httpClient.DefaultRequestHeaders.Authorization = null;
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            }
            AuthenticationState state = new AuthenticationState(new ClaimsPrincipal(identity));
            return state;
        }

        public async Task MarkUserAsAuthenticated(AuthenticationResult authenticationResult)
        {
            await _tokenStorage.SetAccessTokenAsync(authenticationResult.AccessToken);
            await _tokenStorage.SetRefreshTokenAsync(authenticationResult.RefreshToken);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _tokenStorage.RemoveAccessTokensAsync();
            await _tokenStorage.RemoveRefreshTokensAsync();
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private async Task<string> GetAccessTokenAsync()
        {
            AuthenticationResult authenticationResult = await _tokenStorage.GetParametersForRefresh();
            if (!string.IsNullOrWhiteSpace(authenticationResult.AccessToken))
            {
                IEnumerable<Claim> claims = ParseClaimsFromJwt(authenticationResult.AccessToken);
                long jwtExpValue = long.Parse(claims.FirstOrDefault(f => f.Type.Equals("exp")).Value);
                DateTime expirationTime = DateTimeOffset.FromUnixTimeSeconds(jwtExpValue).DateTime;
                if (expirationTime > DateTime.UtcNow)
                {
                    return authenticationResult.AccessToken;
                }
            }
            if (!string.IsNullOrWhiteSpace(authenticationResult.RefreshToken))
            {
                await _tokenStorage.RemoveAccessTokensAsync();
                ApiResponseDetails<AuthenticationResult> response = await _tokenRefreshApiAccessor.Refresh(authenticationResult);
                if (response.IsSuccessStatusCode)
                {
                    await _tokenStorage.RemoveRefreshTokensAsync();
                    await MarkUserAsAuthenticated(response.Result);
                }
            }
            return null;
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string accessToken)
        {
            var claims = new List<Claim>();
            var payload = accessToken.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}