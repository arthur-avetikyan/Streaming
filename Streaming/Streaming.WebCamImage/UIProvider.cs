using Streaming.Composer.Base;
using Streaming.Composer.UI;
using System;
using System.ComponentModel.Composition;

namespace Streaming.WebCamImage
{
    [Export(typeof(IUIViewProviderBase))]
    public class UIProvider : UIViewProviderBase
    {
        public UIProvider()
        {
            Key = "WebCamImage";
            Title = "WebCam Image";
            EntryKey = "WebCamImage";
        }

        [Import("WebCamImageView")]
        public override Lazy<IView> View { get; set; }

        [Import("WebCamImageViewModel")]
        public override Lazy<IViewModel> ViewModel { get; set; }
    }
}
