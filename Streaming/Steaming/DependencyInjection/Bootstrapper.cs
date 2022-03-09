using Microsoft.Extensions.Configuration;

using Splat;

using Streaming.BusinessLogic;
using Streaming.BusinessLogic.Implementations;
using Streaming.BusinessLogic.Interfaces;
using Streaming.BusinessLogic.Models;
using Streaming.ViewModels;
using Streaming.ViewModels.Interfaces;

using System.IO;

namespace Streaming.DependencyInjection
{
    public static class Bootstrapper
    {
        public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            RegisterConfiguration(services, resolver);
            RegisterServices(services, resolver);
            RegisterViewModels(services, resolver);
        }

        private static void RegisterConfiguration(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            services.RegisterConstant<IEnvironmentSetting>(new EnvironmentSetting());

            var path = resolver.GetRequiredService<IEnvironmentSetting>().ContentRootPath;
            FileLogger.Instance.Log(LogTypes.Info, $"Content Root Path: {path}");

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(path, "Settings", "appsettings.json"), optional: true, reloadOnChange: true)
                .Build();

            var applicationSettings = new ApplicationSettings();
            configuration.GetSection("ApplicationSettings").Bind(applicationSettings);
            services.RegisterConstant(applicationSettings);

            services.Register(() => configuration);
        }

        private static void RegisterServices(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            services.RegisterLazySingleton<IConfigurationSetting>(() => new ConfigurationSetting(
                resolver.GetRequiredService<IEnvironmentSetting>()
                ));

            services.RegisterLazySingleton<IRecurringTask>(() => new PluginGetterService(
                resolver.GetRequiredService<ApplicationSettings>(),
                resolver.GetRequiredService<IEnvironmentSetting>()
                ));

            //services.RegisterLazySingleton<IDialogService>(() => new DialogService(
            //    resolver.GetRequiredService<IMainWindowProvider>()
            //));

            //services.RegisterLazySingleton<IMainWindowProvider>(() => new MainWindowProvider());
        }

        private static void RegisterViewModels(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            services.Register(() => new PluginListViewModel(
                resolver.GetRequiredService<ISaver>()//,
                                                     // resolver.GetRequiredService<IDialogService>()
                ));

            services.Register<ISaver>(() => new SettingsDialogViewModel(
                 resolver.GetRequiredService<IConfigurationSetting>(),
                 resolver.GetRequiredService<ApplicationSettings>()
                ));

            services.Register(() => new SettingsDialogViewModel(
                 resolver.GetRequiredService<IConfigurationSetting>(),
                 resolver.GetRequiredService<ApplicationSettings>()
                ));

            services.Register<IRegister>(() => new RegisterViewModel(
                resolver.GetRequiredService<ApplicationSettings>(),
                resolver.GetRequiredService<IConfigurationSetting>()
                ));

            services.Register(() => new MainWindowViewModel(
                resolver.GetRequiredService<ApplicationSettings>(),
                resolver.GetRequiredService<IRecurringTask>(),
                resolver.GetRequiredService<IRegister>(),
                resolver.GetRequiredService<PluginListViewModel>()
            ));
        }
    }
}
