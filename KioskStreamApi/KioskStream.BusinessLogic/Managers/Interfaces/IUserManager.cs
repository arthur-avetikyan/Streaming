
using System.Threading.Tasks;
using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Common.DataTransferObjects.Account;
using KioskStream.Web.Common.DataTransferObjects.UserManagement;
using KioskStream.Web.Server.Wrappers;

namespace KioskStream.BusinessLogic.Managers.Interfaces
{
    public interface IUserManager : IManager<UserDetails, RegisterParameters, int>
    {
        Task<ApiResponse<EmptyResponse>> ChangePassword(ChangePasswordParameters parameters);
        Task<ApiResponse<EmptyResponse>> UpdateAsync(UserDetails userDetails);
    }
}
