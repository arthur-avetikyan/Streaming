
using System.Collections.Generic;
using System.Threading.Tasks;

using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Server.Wrappers;

namespace KioskStream.Web.Server.Managers.Interfaces
{
    public interface IManager<TResponse, in TRequest, in TKey> where TResponse : class
    {
        Task<ApiResponse<TResponse>> GetAsync(TKey key);

        Task<ApiResponse<List<TResponse>>> GetListAsync(int pageSize = 10, int currentPage = 0);

        Task<ApiResponse<TResponse>> CreateAsync(TRequest request);

        Task<ApiResponse<EmptyResponse>> UpdateAsync(TRequest request);

        Task<ApiResponse<EmptyResponse>> DeleteAsync(TKey key);
    }
}
