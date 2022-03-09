
using System.ComponentModel;

using KioskStream.Core.Enums;

namespace KioskStream.Core.Constants.Permissions.Dashboard
{
    public static partial class Dashboard
    {
        public static class User
        {
            [Description("Create User")]
            public const string Create = nameof(Dashboard) + "." + nameof(User) + "." + nameof(EPermissionAction.Create);

            [Description("Read User data")]
            public const string Read = nameof(Dashboard) + "." + nameof(User) + "." + nameof(EPermissionAction.Read);

            [Description("Edit Users")]
            public const string Update = nameof(Dashboard) + "." + nameof(User) + "." + nameof(EPermissionAction.Update);

            [Description("Delete User")]
            public const string Delete = nameof(Dashboard) + "." + nameof(User) + "." + nameof(EPermissionAction.Delete);
        }
    }
}
