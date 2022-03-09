using Streaming.Composer.Base;
using Streaming.Composer.UI;
using System;
using System.ComponentModel.Composition;

namespace Streaming.Drawing
{
    [Export(typeof(IUIViewProviderBase))]
    public class UIProvider : UIViewProviderBase
    {
        public UIProvider()
        {
            Key = "Draw";
            Title = "Rectangle";
            EntryKey = "Draw";
        }

        [Import("DrawView")]
        public override Lazy<IView> View { get; set; }

        [Import("DrawViewModel")]
        public override Lazy<IViewModel> ViewModel { get; set; }
    }
}
