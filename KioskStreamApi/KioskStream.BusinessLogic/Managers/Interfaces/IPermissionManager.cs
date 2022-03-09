using System.Collections.Generic;
using System.Threading.Tasks;

using KioskStream.Web.Common.DataTransferObjects.UserManagement;
using KioskStream.Web.Server.Wrappers;

namespace KioskStream.BusinessLogic.Managers.Interfaces
{
    public interface IPermissionManager
    {
        Task<ApiResponse<string>> CreatePermissionAsync(PermissionDto newPermission);

        Task<ApiResponse<string>> DeletePermissionAsync(string permissionName);

        Task<ApiResponse<PermissionDto>> GetPermissionAsync(string permission);

        Task<ApiResponse<List<string>>> GetPermissionsAsync();
    }
}