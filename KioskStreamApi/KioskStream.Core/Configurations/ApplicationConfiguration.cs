namespace KioskStream.Core.Configurations
{
    public class ApplicationConfiguration
    {
        public string Url { get; set; }

        public ApplicationAuthenticationConfiguration Authentication { get; set; }

        public string TemplatesFilePath { get; set; }
    }
}
