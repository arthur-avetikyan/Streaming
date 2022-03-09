using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KioskStream.Web.Common.DataTransferObjects.UserManagement
{
    public class RoleRequest
    {
        [Range(0, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [StringLength(
            maximumLength: 100,
            ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 2)]
        [DataType(DataType.Text)]
        public string Name { get; set; }


        public List<string> Permissions { get; set; } = new List<string>();
    }
}
