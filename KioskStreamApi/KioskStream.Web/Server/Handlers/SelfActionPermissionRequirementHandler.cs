using KioskStream.Core.Constants.Permissions.Dashboard;
using KioskStream.Core.Security.Authorization;
using KioskStream.Data;
using KioskStream.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KioskStream.Web.Server.Handlers
{
    public class SelfActionPermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<User> _userRepository;

        public SelfActionPermissionRequirementHandler(IRepository<User> userRepository,
                                                      IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null)
                return;

            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Name))
                return;

            var user = await _userRepository.Get(u => u.UserName == context.User.Identity.Name)
                                            .Select(u => new
                                            {
                                                UserId = u.Id,
                                            })
                                            .FirstOrDefaultAsync();
            if (user == null)
                return;

            HttpRequest request = _httpContextAccessor.HttpContext.Request;
        }
    }
}
