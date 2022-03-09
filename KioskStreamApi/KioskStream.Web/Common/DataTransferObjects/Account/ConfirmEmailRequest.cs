
using System.ComponentModel.DataAnnotations;

namespace KioskStream.Web.Common.DataTransferObjects.Account
{
    public class ConfirmEmailRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required] 
        public string Token { get; set; }
    }
}
