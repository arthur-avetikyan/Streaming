using System.Threading.Tasks;

using KioskStream.Web.Common.DataTransferObjects;
using KioskStream.Web.Common.DataTransferObjects.Account;

namespace KioskStream.Web.Client.Services.Interfaces
{
    public interface ITokenRefreshApiAccessor
    {
        Task<ApiResponseDetails<AuthenticationResult>> Refresh(AuthenticationResult authenticationResult);
    }
}