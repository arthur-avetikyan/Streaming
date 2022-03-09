
namespace KioskStream.Mailing
{
    public class ConfirmationEmailParameters
    {
        public string Recipient { get; set; }

        public string UserName { get; set; }

        public string CallbackUrl { get; set; }

        public string UserId { get; set; }

        public string Token { get; set; }

        public string Template { get; set; }
    }
}
