
using System;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;

namespace KioskStream.Web.Server.Services
{
    public class FileContentProvider
    {
        private readonly string _contentRootPath;

        public FileContentProvider()
        {
            var entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
            _contentRootPath = entryAssembly != null
                ? Path.GetDirectoryName(entryAssembly.Location)
                : throw new DirectoryNotFoundException($"{nameof(entryAssembly)} is null");
        }

        public string ReadFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new InvalidOperationException($"{nameof(path)} is not correct");

            IFileInfo fileInfo = new PhysicalFileInfo(new FileInfo(Path.Combine(_contentRootPath, path)));

            if (!fileInfo.Exists)
                throw new FileNotFoundException($"Template file located at \"{path}\" was not found");

            using var fs = fileInfo.CreateReadStream();
            using var sr = new StreamReader(fs);
            return sr.ReadToEnd();
        }
    }
}
