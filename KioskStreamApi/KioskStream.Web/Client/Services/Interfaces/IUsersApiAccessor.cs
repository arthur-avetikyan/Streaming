
using System.Collections.Generic;
using System.Threading.Tasks;

using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Common.DataTransferObjects.Account;
using KioskStream.Web.Common.DataTransferObjects.UserManagement;

namespace KioskStream.Web.Client.Services.Interfaces
{
    public interface IUsersApiAccessor
    {
        Task<ApiResponseDetails<UserDetails>> GetAsync(string userName);

        Task<ApiResponseDetails<List<UserDetails>>> GetListAsync(int pageSize = 10, int currentPage = 0, bool unemployeed = false);

        Task<ApiResponseDetails<EmptyResponse>> CreateAsync(RegisterParameters parameters);

        Task<ApiResponseDetails<EmptyResponse>> UpdateAsync(UserDetails userDetails);

        Task<ApiResponseDetails<EmptyResponse>> DeleteAsync(string userName);

        Task<ApiResponseDetails<EmptyResponse>> ChangePasswordAsync(ChangePasswordParameters parameters);
        Task<UserInfo> GetUserInfoAsync(string userName);
    }
}
