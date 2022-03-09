using System.Net;
using System.Threading.Tasks;

using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Common.DataTransferObjects.Account;

namespace KioskStream.Web.Client.Services.Interfaces
{
    public interface IAuthorizationApiAccessor
    {
        Task<ApiResponseDetails<AuthenticationResult>> Login(LoginRequest loginParameters);

        Task<ApiResponseDetails<string>> Logout();

        Task<ApiResponseDetails<UserDetails>> Register(RegisterParameters registerParameters);

        Task<bool> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest);
        
        // Create

        //ForgotPassword

        //ResetPassword

        //Logout

        //UpdateUser

        //Task<ApiResponseDetails<UserDetails>> GetUserDetails();
    }
}
