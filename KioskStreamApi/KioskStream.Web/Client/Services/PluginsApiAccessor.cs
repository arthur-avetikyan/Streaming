using KioskStream.Web.Client.Services.Interfaces;
using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Common.DataTransferObjects.RequestResponse;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace KioskStream.Web.Client.Services
{
    public class PluginsApiAccessor : IPluginsApiAccessor
    {
        private readonly HttpClient _httpClient;

        public PluginsApiAccessor(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponseDetails<List<PluginResponse>>> GetPluginListAsync(int pageSize = 10, int currentPage = 0) =>
            await _httpClient.GetFromJsonAsync<ApiResponseDetails<List<PluginResponse>>>($"api/Plugins?pageSize={pageSize}&pageNumber={currentPage}");

        public async Task<ApiResponseDetails<PluginResponse>> GetPluginAsync(int pluginId) =>
            await _httpClient.GetFromJsonAsync<ApiResponseDetails<PluginResponse>>($"api/Plugins/{pluginId}");

        public async Task<ApiResponseDetails<EmptyResponse>> CreatePluginAsync(PluginRequest pluginRequest)
        {
            HttpResponseMessage lResponse = await _httpClient.PostAsJsonAsync($"api/Plugins", pluginRequest);
            return JsonConvert.DeserializeObject<ApiResponseDetails<EmptyResponse>>(await lResponse.Content.ReadAsStringAsync());
        }

        public async Task<ApiResponseDetails<EmptyResponse>> UpdatePluginAsync(PluginRequest pluginRequest)
        {
            HttpResponseMessage lResponse = await _httpClient.PutAsJsonAsync($"api/Plugins/{pluginRequest.Id}", pluginRequest);
            return JsonConvert.DeserializeObject<ApiResponseDetails<EmptyResponse>>(await lResponse.Content.ReadAsStringAsync());
        }

        public async Task<ApiResponseDetails<EmptyResponse>> DeletePluginAsync(int pluginId) { return new ApiResponseDetails<EmptyResponse>(); }
    }
}
