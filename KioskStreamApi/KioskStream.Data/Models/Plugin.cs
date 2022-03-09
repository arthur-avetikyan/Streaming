using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KioskStream.Data.Models
{
    public partial class Plugin
    {
        public Plugin()
        {

        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public DateTime CreateDateTimeUtc{ get; set; }

        public IList<KioskPlugin> KioskPlugins { get; set; }
    }
}
