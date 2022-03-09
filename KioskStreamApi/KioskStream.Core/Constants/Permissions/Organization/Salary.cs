using KioskStream.Core.Enums;

using System.ComponentModel;

namespace KioskStream.Core.Constants.Permissions.Organization
{
    public partial class Organization
    {
        public static class Salary
        {
            [Description("Create Salary")]
            public const string Create = nameof(Organization) + "." + nameof(Salary) + "." + nameof(EPermissionAction.Create);

            [Description("Read Salary data")]
            public const string Read = nameof(Organization) + "." + nameof(Salary) + "." + nameof(EPermissionAction.Read);

            [Description("Edit Salary")]
            public const string Update = nameof(Organization) + "." + nameof(Salary) + "." + nameof(EPermissionAction.Update);

            [Description("Delete Salary")]
            public const string Delete = nameof(Organization) + "." + nameof(Salary) + "." + nameof(EPermissionAction.Delete);
        }
    }
}
