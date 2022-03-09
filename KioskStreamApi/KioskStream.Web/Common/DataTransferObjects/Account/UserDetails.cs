
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KioskStream.Web.Common.DataTransferObjects.Account
{
    public class UserDetails
    {
        public bool IsAuthenticated { get; set; }

        public int Id { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [RegularExpression(@"[^\s]+", ErrorMessage = "Spaces are not permitted.")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }
        public int TenantId { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<string> Roles { get; set; }

        public List<KeyValuePair<string, string>> ExposedClaims { get; set; }

        public bool DisableTenantFilter { get; set; }

        public bool DisplayMore { get; set; } = false;

        public UserDetails Clone()
        {
            return (UserDetails) MemberwiseClone();
        }

    }
}
