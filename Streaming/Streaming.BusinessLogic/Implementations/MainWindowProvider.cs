using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

using Streaming.BusinessLogic.Interfaces;

namespace Streaming.BusinessLogic.Implementations
{
    public class MainWindowProvider : IMainWindowProvider
    {
        public Window GetMainWindow()
        {
            var lifetime = (IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime;

            return lifetime.MainWindow;
        }
    }
}