using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;

using Splat;

using Streaming.BusinessLogic;
using Streaming.DependencyInjection;

using System;
using System.IO;

namespace Streaming
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args)
        {
            try
            {
                SetupConfigurationFile();
                SubscribeToDomainUnhandledEvents();
                RegisterDependencies();
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(args, ShutdownMode.OnMainWindowClose);
            }
            catch (Exception ex)
            {
                FileLogger.Instance.Log(LogTypes.Error, ex.Message, ex.StackTrace);
                throw;
            }
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {

            var builder = AppBuilder.Configure<App>()
                                    .UsePlatformDetect()
                                    .With(new X11PlatformOptions { UseGpu = true, UseDBusMenu = true, UseDeferredRendering = true, OverlayPopups = true })
                                    .With(new AvaloniaNativePlatformOptions { UseGpu = true, UseDeferredRendering = true, OverlayPopups = true })
                                    .With(new MacOSPlatformOptions { ShowInDock = true })
                                    .With(new Win32PlatformOptions { UseDeferredRendering = false, UseWindowsUIComposition = true, OverlayPopups = true })
                                    .LogToTrace()
                                    .UseReactiveUI();
            return builder;
        }

        private static void RegisterDependencies() =>
            Bootstrapper.Register(Locator.CurrentMutable, Locator.Current);

        private static void SubscribeToDomainUnhandledEvents() =>
         AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
         {
             var ex = (Exception)args.ExceptionObject;
             FileLogger.Instance.Log(LogTypes.Error, $"Unhandled application error: {ex.Message}", ex.StackTrace);
         };

        private static void SetupConfigurationFile()
        {
            var destinationFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Settings");
            var fileToCreate = Path.Combine(destinationFolder, "appsettings.json");

            if (File.Exists(fileToCreate))
                return;

            var contents = new string[]
            {
                "{",
                "\"ApplicationSettings\": {",
                "\"ApiBaseUrl\": \"https://localhost:5001/\",",
                "\"KioskIdentifier\": \"00000000000000000000000000000000\"",
                "}",
                "}"
            };
            Directory.CreateDirectory(destinationFolder);
            File.WriteAllLines(fileToCreate, contents);
        }
    }
}
