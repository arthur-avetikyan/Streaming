namespace Streaming.WebCamImage.Services
{
    interface IWebCamImageProvider
    {
        string TakeImage(string imageFullPath);
    }
}
