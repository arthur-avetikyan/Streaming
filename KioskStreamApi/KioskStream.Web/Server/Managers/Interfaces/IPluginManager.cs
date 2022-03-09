using KioskStream.Web.Common.DataTransferObjects.RequestResponse;
using KioskStream.Web.Server.Wrappers;

using System;
using System.Threading.Tasks;

namespace KioskStream.Web.Server.Managers.Interfaces
{
    public interface IPluginManager : IManager<PluginResponse, PluginRequest, int>
    {
        Task<ApiResponse<PluginResponse>> GetAsync(Guid kioskIdentifier, int key);
    }
}
