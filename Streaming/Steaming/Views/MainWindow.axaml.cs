using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using System.Linq;

namespace Streaming.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            HandleWindowStateChanged(WindowState.FullScreen);
        }

        private void InitializeComponent()
        {
            PlatformImpl.SetSystemDecorations(SystemDecorations.None);

            //WindowStartupLocation = WindowStartupLocation.Manual;
            //var screen = PlatformImpl.Screen.AllScreens.FirstOrDefault();
            //Height = screen.Bounds.Height - 20;
            //Width = screen.Bounds.Width;
            //Position = screen.Bounds.Position.WithY(20);

            //PlatformImpl.CanResize(false);
            //PlatformImpl.SetTopmost(true);
            //PlatformImpl.Closing = () => true;

            AvaloniaXamlLoader.Load(this);
        }
    }
}