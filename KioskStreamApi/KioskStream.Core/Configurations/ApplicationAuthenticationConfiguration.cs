namespace KioskStream.Core.Configurations
{
    public class ApplicationAuthenticationConfiguration
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public double ExpiryDurationMinutes { get; set; }
    }
}
