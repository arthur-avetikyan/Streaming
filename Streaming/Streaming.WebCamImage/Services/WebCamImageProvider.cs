
using System;
using System.IO;
using System.Linq;

namespace Streaming.WebCamImage.Services
{
    class WebCamImageProvider : IWebCamImageProvider
    {
        private const string _capture = @"capture.png";

        public string TakeImage(string imageFullPath)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Assets", "Images");
            Directory.CreateDirectory(path);
            string newFileName = Path.Combine(path, $"{Guid.NewGuid()}{_capture}");
            File.Copy(imageFullPath, newFileName);

            foreach (var item in Directory.GetFiles(path).Where(f => f.EndsWith(".png") && !f.Contains(newFileName)))
            {
                File.Delete(item);
            }
            return newFileName;
        }
    }
}
