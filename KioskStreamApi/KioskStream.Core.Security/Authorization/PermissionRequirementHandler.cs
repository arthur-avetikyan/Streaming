using KioskStream.Data;
using KioskStream.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KioskStream.Core.Security.Authorization
{
    public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IRepository<User> _userRepository;

        public PermissionRequirementHandler(
            IRepository<User> userRepository
         )
        {
            _userRepository = userRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null)
                return;

            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Name))
                return;

            var user = await _userRepository.Get(u => u.UserName == context.User.Identity.Name).Select(u => new { u.Id, u.UserName }).FirstOrDefaultAsync();
            if (user == null)
                return;

            //var userPermissionCodes = await _userRoleRepository
            //    .Get(ur => ur.UserId == user.Id)
            //    .SelectMany(item =>
            //    item.Role.RolePermissions.Select(rp => rp.Permission.Code))
            //    .ToListAsync();

            //if (userPermissionCodes.Any(code => code == requirement.Permission))
            //    context.Succeed(requirement);
            //else
            //    return;
        }
    }
}
