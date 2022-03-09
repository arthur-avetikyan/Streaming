using Streaming.Composer.Base;
using Streaming.Composer.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace Streaming.Ellipse
{
    [Export(typeof(IUIViewProviderBase))]
    public class UIProvider : UIViewProviderBase
    {
        public UIProvider()
        {
            Key = "Ellipse";
            Title = "Ellipse";
            EntryKey = "Ellipse";
        }

        [Import("EllipseView")]
        public override Lazy<IView> View { get; set; }

        [Import("EllipseViewModel")]
        public override Lazy<IViewModel> ViewModel { get; set; }
    }
}
