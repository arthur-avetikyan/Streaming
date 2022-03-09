using KioskStream.Core.Enums;

using System.ComponentModel;

namespace KioskStream.Core.Constants.Permissions.Feedback
{
    public static partial class Feedback
    {
        public static class Review
        {
            [Description("Create Review")]
            public const string Create = nameof(Feedback) + "." + nameof(Review) + "." + nameof(EPermissionAction.Create);

            [Description("Read Review data")]
            public const string Read = nameof(Feedback) + "." + nameof(Review) + "." + nameof(EPermissionAction.Read);

            [Description("Edit Review")]
            public const string Update = nameof(Feedback) + "." + nameof(Review) + "." + nameof(EPermissionAction.Update);

            [Description("Delete Review")]
            public const string Delete = nameof(Feedback) + "." + nameof(Review) + "." + nameof(EPermissionAction.Delete);
        }
    }
}
