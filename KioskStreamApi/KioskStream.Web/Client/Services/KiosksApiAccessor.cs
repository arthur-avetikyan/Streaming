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
    public class KiosksApiAccessor : IKiosksApiAccessor
    {
        private readonly HttpClient _httpClient;

        public KiosksApiAccessor(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponseDetails<List<KioskResponse>>> GetKiosksListAsync(int pageSize = 10, int currentPage = 0) =>
            await _httpClient.GetFromJsonAsync<ApiResponseDetails<List<KioskResponse>>>($"api/Kiosks?pageSize={pageSize}&pageNumber={currentPage}");

        public async Task<ApiResponseDetails<KioskResponse>> GetKioskAsync(int kioskId) =>
            await _httpClient.GetFromJsonAsync<ApiResponseDetails<KioskResponse>>($"api/kiosks/{kioskId}");

        public async Task<ApiResponseDetails<EmptyResponse>> UpdateKioskAsync(KioskRequest kioskRequest)
        {
            HttpResponseMessage lResponse = await _httpClient.PutAsJsonAsync($"api/Kiosks/{kioskRequest.Id}", kioskRequest);
            return JsonConvert.DeserializeObject<ApiResponseDetails<EmptyResponse>>(await lResponse.Content.ReadAsStringAsync());
        }
    }
}
