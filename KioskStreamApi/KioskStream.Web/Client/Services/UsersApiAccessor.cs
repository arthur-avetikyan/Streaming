using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using KioskStream.Web.Client.Services.Interfaces;
using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Common.DataTransferObjects.Account;
using KioskStream.Web.Common.DataTransferObjects.UserManagement;

using Microsoft.AspNetCore.Components.Authorization;

using Newtonsoft.Json;

namespace KioskStream.Web.Client.Services
{
    public class UsersApiAccessor : IUsersApiAccessor
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenStorage _tokenStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public UsersApiAccessor(HttpClient httpClient, ITokenStorage tokenStorage, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _tokenStorage = tokenStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<ApiResponseDetails<UserDetails>> GetAsync(string userName)
        {
            return await _httpClient.GetFromJsonAsync<ApiResponseDetails<UserDetails>>($"api/users/{userName}");
        }

        public async Task<UserInfo> GetUserInfoAsync(string userName)
        {
            return await _httpClient.GetFromJsonAsync<UserInfo>($"api/users/{userName}/userInfo");
        }

        public async Task<ApiResponseDetails<List<UserDetails>>> GetListAsync(int pageSize = 10, int currentPage = 0, bool unemployeed = false)
        {
            return await _httpClient.GetFromJsonAsync<ApiResponseDetails<List<UserDetails>>>($"api/users?pageSize={pageSize}&pageNumber={currentPage}&unemployeed={unemployeed}");
        }

        public async Task<ApiResponseDetails<EmptyResponse>> CreateAsync(RegisterParameters parameters)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/users", parameters);
            return JsonConvert.DeserializeObject<ApiResponseDetails<EmptyResponse>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResponseDetails<EmptyResponse>> UpdateAsync(UserDetails userDetails)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/users/{userDetails.Id}", userDetails);
            var result = JsonConvert.DeserializeObject<ApiResponseDetails<EmptyResponse>>(await response.Content.ReadAsStringAsync());
            await _tokenStorage.RemoveAccessTokensAsync();
            await ((IdentityAuthenticationStateProvider)_authenticationStateProvider).GetAuthenticationStateAsync();
            return result;
        }

        public async Task<ApiResponseDetails<EmptyResponse>> DeleteAsync(string userName)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/users/{userName}");
            return JsonConvert.DeserializeObject<ApiResponseDetails<EmptyResponse>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResponseDetails<EmptyResponse>> ChangePasswordAsync(ChangePasswordParameters parameters)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/users/{parameters.UserName}/changePassword", parameters);
            return JsonConvert.DeserializeObject<ApiResponseDetails<EmptyResponse>>(await response.Content.ReadAsStringAsync());
        }
    }
}
