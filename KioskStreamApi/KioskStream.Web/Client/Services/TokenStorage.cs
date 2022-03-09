using System.Threading.Tasks;

using KioskStream.Web.Client.Services.Interfaces;
using KioskStream.Web.Common.DataTransferObjects.Account;

using Blazored.LocalStorage;
using Blazored.SessionStorage;

namespace KioskStream.Web.Client.Services
{
    public class TokenStorage : ITokenStorage
    {
        private readonly ISessionStorageService _sessionStorageService;
        private readonly ILocalStorageService _localStorageService;

        public TokenStorage(ISessionStorageService sessionStorageService, ILocalStorageService localStorageService)
        {
            _sessionStorageService = sessionStorageService;
            _localStorageService = localStorageService;
        }

        public async Task<AuthenticationResult> GetParametersForRefresh()
        {
            AuthenticationResult authenticationTokens = new AuthenticationResult
            {
                RefreshToken = await _localStorageService.GetItemAsync<string>("refreshToken"),
                AccessToken = await _sessionStorageService.GetItemAsync<string>("accessToken")
            };
            return authenticationTokens;
        }

        public async Task SetAccessTokenAsync(string token)
        {
            await _sessionStorageService.SetItemAsync("accessToken", token);
        }

        public async Task SetRefreshTokenAsync(string token)
        {
            await _localStorageService.SetItemAsync("refreshToken", token);
        }

        public async Task RemoveAccessTokensAsync()
        {
            await _sessionStorageService.RemoveItemAsync("accessToken");
        }

        public async Task RemoveRefreshTokensAsync()
        {
            await _localStorageService.RemoveItemAsync("refreshToken");
        }
    }
}
