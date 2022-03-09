
using System;
using KioskStream.Core.Enums;

namespace KioskStream.Core.Extensions
{
    public static class AuthorizationRolesExtensions
    {
        public static string GetAuthorizationRolesName(this int state)
        {
            return Enum.GetName(typeof(EAuthorizationRoles), state);
        }

        public static string GetAuthorizationRolesName(this EAuthorizationRoles state)
        {
            return Enum.GetName(typeof(EAuthorizationRoles), state);
        }
    }
}
