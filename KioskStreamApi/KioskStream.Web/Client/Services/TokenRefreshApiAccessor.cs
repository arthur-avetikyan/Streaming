using KioskStream.Web.Client.Services.Interfaces;
using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Common.DataTransferObjects.Account;

using Newtonsoft.Json;

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace KioskStream.Web.Client.Services
{
    public class TokenRefreshApiAccessor : ITokenRefreshApiAccessor
    {
        private readonly HttpClient _httpClient;

        public TokenRefreshApiAccessor(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponseDetails<AuthenticationResult>> Refresh(AuthenticationResult authenticationResult)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/account/refreshToken", authenticationResult);
            return JsonConvert.DeserializeObject<ApiResponseDetails<AuthenticationResult>>(await response.Content.ReadAsStringAsync());
        }
    }
}