using KioskStream.Web.Common.DataTransferObjects.RequestResponse;
using KioskStream.Web.Server.Wrappers;

using System;
using System.Threading.Tasks;

namespace KioskStream.Web.Server.Managers.Interfaces
{
    public interface IKioskPluginsManager : IManager<KioskPluginResponse, KioskPluginRequest, int>
    {
        Task<ApiResponse<KioskPluginResponse>> GetAsync(Guid kioskIdentifier);
    }
}
