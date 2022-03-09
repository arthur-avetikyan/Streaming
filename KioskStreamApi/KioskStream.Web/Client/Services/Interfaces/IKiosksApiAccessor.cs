using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Common.DataTransferObjects.RequestResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KioskStream.Web.Client.Services.Interfaces
{
    public interface IKiosksApiAccessor
    {
        Task<ApiResponseDetails<KioskResponse>> GetKioskAsync(int kioskId);
        Task<ApiResponseDetails<List<KioskResponse>>> GetKiosksListAsync(int pageSize = 10, int currentPage = 0);
        Task<ApiResponseDetails<EmptyResponse>> UpdateKioskAsync(KioskRequest kioskRequest);
        }
}
