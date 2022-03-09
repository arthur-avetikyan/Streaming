using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

using KioskStream.Web.Client.Services.Interfaces;
using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Common.DataTransferObjects.Account;

using Microsoft.AspNetCore.Components.Authorization;

using Newtonsoft.Json;

namespace KioskStream.Web.Client.Services
{
    public class AuthorizationApiAccessor : IAuthorizationApiAccessor
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthorizationApiAccessor(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<ApiResponseDetails<AuthenticationResult>> Login(LoginRequest loginRequest)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/account/login", loginRequest);
            ApiResponseDetails<AuthenticationResult> loginResult = System.Text.Json.JsonSerializer.Deserialize<ApiResponseDetails<AuthenticationResult>>(
                await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (loginResult.IsSuccessStatusCode)
            {
                await ((IdentityAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginResult.Result);
            }
            return loginResult;
        }

        public async Task<ApiResponseDetails<UserDetails>> Register(RegisterParameters registerParameters)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/account/register", registerParameters);
            return JsonConvert.DeserializeObject<ApiResponseDetails<UserDetails>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<bool> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/account/confirmEmail", confirmEmailRequest);
            return response.StatusCode switch
            {
                HttpStatusCode.BadRequest => false,
                HttpStatusCode.NotFound => false,
                HttpStatusCode.OK => true,
                _ => false
            };
        }

        public async Task<ApiResponseDetails<string>> Logout()
        {
            HttpResponseMessage response = await _httpClient.PostAsync("api/account/logout", null);
            ApiResponseDetails<string> logoutResult = JsonConvert.DeserializeObject<ApiResponseDetails<string>>(await response.Content.ReadAsStringAsync());

            if (logoutResult.IsSuccessStatusCode)
            {
                await ((IdentityAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            }
            return logoutResult;
        }

        //public Task<ApiResponseDetails<UserDetails>> GetUserDetails()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
