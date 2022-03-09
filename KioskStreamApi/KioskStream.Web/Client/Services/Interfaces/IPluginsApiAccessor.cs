using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Common.DataTransferObjects.RequestResponse;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace KioskStream.Web.Client.Services.Interfaces
{
    public interface IPluginsApiAccessor
    {
        Task<ApiResponseDetails<EmptyResponse>> CreatePluginAsync(PluginRequest pluginRequest);
        Task<ApiResponseDetails<EmptyResponse>> DeletePluginAsync(int pluginId);
        Task<ApiResponseDetails<PluginResponse>> GetPluginAsync(int pluginId);
        Task<ApiResponseDetails<List<PluginResponse>>> GetPluginListAsync(int pageSize = 10, int currentPage = 0);
        Task<ApiResponseDetails<EmptyResponse>> UpdatePluginAsync(PluginRequest pluginRequest);
    }
}
