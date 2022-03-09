using Streaming.Composer.Base;

using System;

namespace Streaming.Composer.UI
{
    public abstract class UIViewProviderBase : UIProviderBase, IUIViewProviderBase
    {
        public abstract Lazy<IView> View { get; set; }
        public abstract Lazy<IViewModel> ViewModel { get; set; }
    }
}
