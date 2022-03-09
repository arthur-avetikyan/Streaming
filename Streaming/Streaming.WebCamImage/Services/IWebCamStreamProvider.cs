using System;

namespace Streaming.WebCamImage.Services
{
    interface IWebCamStreamProvider : IDisposable
    {
        void ClearOldFrames();
        string GetFrame();
    }
}
