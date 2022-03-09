
using System;

namespace KioskStream.Web.Common.DataTransferObjects.RequestResponse
{
    public class PluginResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public DateTime CreateDateTimeUtc { get; set; }

        public byte[] File { get; set; }
    }
}
