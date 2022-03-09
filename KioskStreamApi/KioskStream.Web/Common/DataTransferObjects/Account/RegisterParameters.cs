
using System.ComponentModel.DataAnnotations;

namespace KioskStream.Web.Common.DataTransferObjects.Account
{
    public class RegisterParameters
    {
        [Required]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 2)]
        [RegularExpression(@"[^\s]+", ErrorMessage = "Spaces are not permitted.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 2)]
        [RegularExpression(@"[^\s]+", ErrorMessage = "Spaces are not permitted.")]
        [Display(Name = "First Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 2)]
        [RegularExpression(@"[^\s]+", ErrorMessage = "Spaces are not permitted.")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
        //    MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string PasswordConfirm { get; set; }
    }
}
