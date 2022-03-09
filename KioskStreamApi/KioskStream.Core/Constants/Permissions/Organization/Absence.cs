using KioskStream.Core.Enums;

using System.ComponentModel;

namespace KioskStream.Core.Constants.Permissions.Organization
{
    public partial class Organization
    {
        public static class Absence
        {
            [Description("Create Absence")]
            public const string Create = nameof(Organization) + "." + nameof(Absence) + "." + nameof(EPermissionAction.Create);

            [Description("Read Absence data")]
            public const string Read = nameof(Organization) + "." + nameof(Absence) + "." + nameof(EPermissionAction.Read);

            [Description("Edit Absence")]
            public const string Update = nameof(Organization) + "." + nameof(Absence) + "." + nameof(EPermissionAction.Update);

            [Description("Delete Absence")]
            public const string Delete = nameof(Organization) + "." + nameof(Absence) + "." + nameof(EPermissionAction.Delete);

            [Description("Approve Absence")]
            public const string Approve = nameof(Organization) + "." + nameof(Absence) + "." + nameof(EPermissionAction.Approve);

            [Description("Reject Absence")]
            public const string Reject = nameof(Organization) + "." + nameof(Absence) + "." + nameof(EPermissionAction.Reject);
        }
    }
}
