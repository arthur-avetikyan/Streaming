
using System.ComponentModel.DataAnnotations;

namespace KioskStream.Web.Common.DataTransferObjects.RequestResponse
{
    public class PluginRequest
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string FileName { get; set; }

        public byte[] Plugin { get; set; }
    }
}
