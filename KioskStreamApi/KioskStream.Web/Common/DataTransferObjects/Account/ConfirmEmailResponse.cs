
namespace KioskStream.Web.Common.DataTransferObjects.Account
{
    public class ConfirmEmailResponse
    {
        public string UserName { get; set; }

        public int UserId { get; set; }

        public bool IsConfirmed { get; set; }
    }
}
