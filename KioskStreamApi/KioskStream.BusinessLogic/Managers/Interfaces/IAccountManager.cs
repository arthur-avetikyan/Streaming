
using System.Threading.Tasks;
using KioskStream.Web.Common.DataTransferObjects.Account;
using KioskStream.Web.Server.Wrappers;

namespace KioskStream.BusinessLogic.Managers.Interfaces
{
    public interface IAccountManager
    {
        Task<ApiResponse<AuthenticationResult>> Login(LoginRequest parameters);

        Task<ApiResponse<RegisterParameters>> Register(RegisterParameters parameters);

        Task<ApiResponse<ForgotPasswordParameters>> ForgotPassword(ForgotPasswordParameters parameters);

        Task<ApiResponse<AuthenticationResult>> Refresh(AuthenticationResult authenticationResult);

        Task<ApiResponse<string>> Revoke(string userName);
    }
}
