using KioskStream.Core.Enums;

using System.ComponentModel;

namespace KioskStream.Core.Constants.Permissions.Feedback
{
    public static partial class Feedback
    {
        public static class Template
        {
            [Description("Create Template")]
            public const string Create = nameof(Feedback) + "." + nameof(Template) + "." + nameof(EPermissionAction.Create);

            [Description("Read Template data")]
            public const string Read = nameof(Feedback) + "." + nameof(Template) + "." + nameof(EPermissionAction.Read);

            [Description("Edit Template")]
            public const string Update = nameof(Feedback) + "." + nameof(Template) + "." + nameof(EPermissionAction.Update);

            [Description("Delete Template")]
            public const string Delete = nameof(Feedback) + "." + nameof(Template) + "." + nameof(EPermissionAction.Delete);
        }
    }
}
