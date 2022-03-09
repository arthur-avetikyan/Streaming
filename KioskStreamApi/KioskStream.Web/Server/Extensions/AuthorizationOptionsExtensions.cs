using KioskStream.Core.Enums;
using KioskStream.Core.Security.Authorization;

using Microsoft.AspNetCore.Authorization;

namespace KioskStream.Web.Server.Extensions
{
    public static class AuthorizationOptionsExtensions
    {
        public static AuthorizationOptions AddApplicationStaticPolicies(this AuthorizationOptions options)
        {
            options.AddPolicy(nameof(EAuthorizationRoles.Administrator), PolicyStaticProvider.GetAdminOnlyPolicy());
            options.AddPolicy(nameof(EAuthorizationRoles.User), PolicyStaticProvider.GetUserOnlyPolicy());

            return options;
        }
    }
}
