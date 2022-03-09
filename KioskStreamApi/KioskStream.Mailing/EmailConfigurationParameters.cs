using System.Collections.Generic;

namespace KioskStream.Mailing
{
    public class EmailConfigurationParameters
    {
        public EmailConfigurationParameters()
        {
            DestinationAddresses = new List<string>();
            SourceAddresses = new List<string>();
            CcAddresses = new List<string>();
            BccAddresses = new List<string>();
        }

        public List<string> DestinationAddresses { get; set; }
        
        public List<string> SourceAddresses { get; set; }
        
        public List<string> BccAddresses { get; set; }
        
        public List<string> CcAddresses { get; set; }
        
        public string Subject { get; set; }
        
        public string Body { get; set; }
        
        public bool IsHtml { get; set; } = true;
    }
}
