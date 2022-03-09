using System;
using System.ComponentModel.DataAnnotations;

namespace KioskStream.Web.Common.DataTransferObjects.RequestResponse
{
    public class KioskRequest
    {
        public int Id { get; set; }

        [Required]
        [Range(-12, 12, ErrorMessage = "Time zone can be from -12 till 12 from UTC.")]
        public int TimeZone { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Guid KioskIdentifier { get; set; }

        [Required]
        public string Location { get; set; }

        public bool Approved { get; set; }
    }
}
