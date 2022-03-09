
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Common.DataTransferObjects.Account;
using KioskStream.Web.Common.DataTransferObjects.UserManagement;
using KioskStream.Web.Server.Wrappers;

namespace KioskStream.Web.Server.Managers.Interfaces
{
    public interface IUserManager : IManager<UserDetails, RegisterParameters, string>
    {
        //Task<ApiResponse<UserDetails>> GetAsync(string userName);

        Task<ApiResponse<EmptyResponse>> ChangePassword(ChangePasswordParameters parameters);
        Task<UserInfo> GetUserInfoAsync(string userName);
    }
}
