using System.Threading.Tasks;

using KioskStream.Web.Common.DataTransferObjects.Account;

namespace KioskStream.Web.Client.Services.Interfaces
{
    public interface ITokenStorage
    {
        Task<AuthenticationResult> GetParametersForRefresh();
        Task RemoveAccessTokensAsync();
        Task RemoveRefreshTokensAsync();
        Task SetAccessTokenAsync(string token);
        Task SetRefreshTokenAsync(string token);
    }
}