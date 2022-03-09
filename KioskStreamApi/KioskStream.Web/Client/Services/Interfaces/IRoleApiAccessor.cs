using System.Collections.Generic;
using System.Threading.Tasks;

using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Common.DataTransferObjects.UserManagement;

namespace KioskStream.Web.Client.Services.Interfaces
{
    public interface IRoleApiAccessor
    {
        Task<ApiResponseDetails<EmptyResponse>> DeleteRoleAsync(string roleName);

        Task<ApiResponseDetails<List<string>>> GetPremissions();

        Task<ApiResponseDetails<RoleResponse>> GetRoleAsync(string roleName);

        Task<ApiResponseDetails<List<RoleResponse>>> GetRolesAsync(int pageSize = 10, int currentPage = 0);

        Task<ApiResponseDetails<EmptyResponse>> InsertRoleAsync(RoleResponse request);

        Task<ApiResponseDetails<EmptyResponse>> UpdateRoleAsync(RoleResponse request);
    }
}
