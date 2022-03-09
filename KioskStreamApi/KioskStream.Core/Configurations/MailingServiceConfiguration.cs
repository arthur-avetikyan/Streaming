
namespace KioskStream.Core.Configurations
{
    public class MailingServiceConfiguration
    {
        public string SmtpServer { get; set; }
        
        public int SmtpPort { get; set; }
        
        public string SmtpUsername { get; set; }
        
        public string SmtpPassword { get; set; }
        
        public bool SmtpUseSsl { get; set; }

        public string FromName { get; set; }
        
        public string FromAddress { get; set; }
    }
}
