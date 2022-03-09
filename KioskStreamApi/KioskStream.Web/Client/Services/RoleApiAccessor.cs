using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using KioskStream.Web.Client.Services.Interfaces;
using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Common.DataTransferObjects.UserManagement;

using Newtonsoft.Json;

namespace KioskStream.Web.Client.Services
{
    public class RoleApiAccessor : IRoleApiAccessor
    {
        private readonly HttpClient _httpClient;

        public RoleApiAccessor(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponseDetails<List<RoleResponse>>> GetRolesAsync(int pageSize = 10, int currentPage = 0)
            => await _httpClient.GetFromJsonAsync<ApiResponseDetails<List<RoleResponse>>>($"api/roles?pageSize={pageSize}&pageNumber={currentPage}");

        public async Task<ApiResponseDetails<RoleResponse>> GetRoleAsync(string roleName)
            => await _httpClient.GetFromJsonAsync<ApiResponseDetails<RoleResponse>>($"api/roles/{roleName}");

        public async Task<ApiResponseDetails<List<string>>> GetPremissions()
            => await _httpClient.GetFromJsonAsync<ApiResponseDetails<List<string>>>("api/permissions");

        public async Task<ApiResponseDetails<EmptyResponse>> InsertRoleAsync(RoleResponse request)
        {
            HttpResponseMessage lResponse = await _httpClient.PostAsJsonAsync("api/roles", request);
            return JsonConvert.DeserializeObject<ApiResponseDetails<EmptyResponse>>(await lResponse.Content.ReadAsStringAsync());
        }

        public async Task<ApiResponseDetails<EmptyResponse>> UpdateRoleAsync(RoleResponse request)
        {
            HttpResponseMessage lResponse = await _httpClient.PutAsJsonAsync("api/roles", request);
            return JsonConvert.DeserializeObject<ApiResponseDetails<EmptyResponse>>(await lResponse.Content.ReadAsStringAsync());
        }

        public async Task<ApiResponseDetails<EmptyResponse>> DeleteRoleAsync(string roleName)
        {
            HttpResponseMessage lResponse = await _httpClient.DeleteAsync($"api/roles/{roleName}");
            return JsonConvert.DeserializeObject<ApiResponseDetails<EmptyResponse>>(await lResponse.Content.ReadAsStringAsync());
        }
    }
}
