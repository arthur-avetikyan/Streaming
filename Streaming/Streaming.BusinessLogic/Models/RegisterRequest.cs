using System;

namespace Streaming.BusinessLogic.Models
{
    public class RegisterRequest
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public Guid KioskIdentifier { get; set; }
        public int TimeZone { get; set; }
    }
}
