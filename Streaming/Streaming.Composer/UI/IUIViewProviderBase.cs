using Streaming.Composer.Base;

using System;

namespace Streaming.Composer.UI
{
    public interface IUIViewProviderBase : IUIProviderBase
    {
        Lazy<IView> View { get; set; }
        Lazy<IViewModel> ViewModel { get; set; }
    }
}
