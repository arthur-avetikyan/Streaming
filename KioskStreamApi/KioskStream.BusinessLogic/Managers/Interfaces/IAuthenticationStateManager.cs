using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using KioskStream.Models;
using KioskStream.Web.Common.DataTransferObjects.Account;

namespace KioskStream.BusinessLogic.Managers.Interfaces
{
    public interface IAuthenticationStateManager
    {
        Task<AuthenticationResult> Authenticate(int userId, int? refreshTokenId, IEnumerable<Claim> claims);

        IEnumerable<Claim> GetClaims(User user);
    }
}