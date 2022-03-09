using System;


namespace KioskStream.Web.Common.DataTransferObjects.RequestResponse
{
    public class KioskResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Guid KioskIdentifier { get; set; }

        public string Location { get; set; }

        public int TimeZone { get; set; }

        public DateTime CreateDateTimeUtc { get; set; }

        public bool Approved { get; set; }
    }
}
