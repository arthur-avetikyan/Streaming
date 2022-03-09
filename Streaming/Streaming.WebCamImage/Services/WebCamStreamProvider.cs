
using OpenCvSharp;

using System;
using System.IO;
using System.Linq;

namespace Streaming.WebCamImage.Services
{
    class WebCamStreamProvider : IWebCamStreamProvider
    {
        private const string _stream = @"stream.png";
        private readonly string _path;
        private VideoCapture _capture;
        private bool disposedValue;

        public WebCamStreamProvider()
        {
            _capture = new VideoCapture(0, VideoCaptureAPIs.ANY);
            _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Assets", "Stream");
            Directory.CreateDirectory(_path);
        }

        public string GetFrame()
        {
            var fileName = Path.Combine(_path, $"{Guid.NewGuid()}{_stream}");
            using var image = new Mat();
            _capture.Read(image);

            if (image.Empty()) return null;
            if (!image.SaveImage(fileName)) return null;

            return fileName;
        }

        public void ClearOldFrames()
        {
            var files = Directory.GetFiles(_path).ToList();
            try
            {
                foreach (var file in files)
                    File.Delete(file);
            }
            catch (Exception)
            {
            }
        }

        #region IDisposable pattern

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    if (!_capture.IsDisposed)
                    {
                        _capture?.Release();
                        _capture?.Dispose();
                    }

                    if (Directory.Exists(_path))
                    {
                        foreach (var file in Directory.GetFiles(_path).ToList())
                            File.Delete(file);
                    }
                }
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~WebCamStreamProvider()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
