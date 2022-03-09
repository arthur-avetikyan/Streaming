using System;
using System.ComponentModel.DataAnnotations;

namespace KioskStream.Data.Models
{
    public partial class KioskPlugin
    {
        public KioskPlugin()
        {
            
        }

        [Key]
        public int Id { get; set; }

        public virtual int PluginId{ get; set; }

        public virtual int KioskId{ get; set; }

        public Kiosk Kiosk { get; set; }

        public Plugin Plugin { get; set; }
    }
}
