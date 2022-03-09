using System;
using System.Collections.Generic;

namespace KioskStream.Web.Common.DataTransferObjects.UserManagement
{
    public class RoleResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<string> Permissions { get; set; }

        public bool DisplayMore { get; set; } = false;

        public string FormattedPermissions
        {
            get
            {
                return String.Join(", ", Permissions.ToArray());
            }
        }
    }
}
