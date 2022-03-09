using Streaming.BusinessLogic.Interfaces;

using System;
using System.IO;

namespace Streaming.BusinessLogic.Implementations
{
    public class EnvironmentSetting : IEnvironmentSetting
    {
        private readonly string _contentRootFolder;

        public EnvironmentSetting()
        {
            //if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            //{
            //    _contentRootFolder = Directory.GetCurrentDirectory();
            //}

            //if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            //{
            //    _contentRootFolder = Directory.GetCurrentDirectory();
            //}

            //if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            //{
            //    _contentRootFolder = Directory.GetCurrentDirectory();
            //}
            //_contentRootFolder = Directory.GetCurrentDirectory();
            _contentRootFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        public string ContentRootPath { get => _contentRootFolder; }
    }
}
