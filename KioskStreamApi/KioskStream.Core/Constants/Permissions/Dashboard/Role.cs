
using KioskStream.Core.Enums;
using System.ComponentModel;

namespace KioskStream.Core.Constants.Permissions.Dashboard
{
    public static partial class Dashboard
    {
        public static class Role
        {
            [Description("Create Role")]
            public const string Create = nameof(Dashboard) + "." + nameof(Role) + "." + nameof(EPermissionAction.Create);

            [Description("Read Roles data")]
            public const string Read = nameof(Dashboard) + "." + nameof(Role) + "." + nameof(EPermissionAction.Read);

            [Description("Edit Roles")]
            public const string Update = nameof(Dashboard) + "." + nameof(Role) + "." + nameof(EPermissionAction.Update);

            [Description("Delete Role")]
            public const string Delete = nameof(Dashboard) + "." + nameof(Role) + "." + nameof(EPermissionAction.Delete);
        }
    }
}
