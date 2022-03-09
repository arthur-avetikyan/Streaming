using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KioskStream.Data.Models
{
    public partial class Kiosk
    {
        public Kiosk()
        {
            
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public Guid KioskIdentifier { get; set; }

        public string Location { get; set; }

        public int TimeZone { get; set; }

        public DateTime CreateDateTimeUtc{ get; set; }

        public bool Approved { get; set; }

        public IList<KioskPlugin> KioskPlugins { get; set; }

    }
}
