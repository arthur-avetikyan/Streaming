
using System.ComponentModel.DataAnnotations;

namespace KioskStream.Web.Common.DataTransferObjects.Account
{
    public class ForgotPasswordParameters
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
