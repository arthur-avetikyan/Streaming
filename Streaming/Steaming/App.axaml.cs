using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using Splat;

using Streaming.DependencyInjection;
using Streaming.Styles.Themes;
using Streaming.ViewModels;
using Streaming.Views;

namespace Streaming
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            LoadTheme();
        }


        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow { DataContext = GetRequiredService<MainWindowViewModel>() };
                desktop.MainWindow.WindowStartupLocation = WindowStartupLocation.Manual;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void LoadTheme() => Styles.Add(new DarkTheme());

        private static T GetRequiredService<T>() => Locator.Current.GetRequiredService<T>();
    }
}
