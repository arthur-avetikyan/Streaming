using KioskStream.Web.Client.Services;
using KioskStream.Web.Client.Services.Interfaces;

using Blazored.LocalStorage;
using Blazored.SessionStorage;

using MatBlazor;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace KioskStream.Web.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            ConfigureServices(builder.Services, builder.HostEnvironment);

            await builder.Build().RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services, IWebAssemblyHostEnvironment hostEnvironment)
        {
            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(hostEnvironment.BaseAddress) });

            services.AddOptions();
            services.AddAuthenticationCore();
            services.AddAuthorizationCore();

            services.AddBlazoredSessionStorage();
            services.AddBlazoredLocalStorage();

            services.AddMatToaster(config =>
            {
                config.Position = MatToastPosition.BottomRight;
                config.PreventDuplicates = true;
                config.NewestOnTop = true;
                config.ShowCloseButton = true;
                config.MaximumOpacity = 95;
                config.VisibleStateDuration = 2500;
            });
            services.AddScoped<ITokenRefreshApiAccessor, TokenRefreshApiAccessor>();
            services.AddScoped<AuthenticationStateProvider, IdentityAuthenticationStateProvider>();
            services.AddScoped<IAuthorizationApiAccessor, AuthorizationApiAccessor>();
            services.AddScoped<ITokenStorage, TokenStorage>();

            services.AddScoped<IRoleApiAccessor, RoleApiAccessor>();
            services.AddScoped<IUsersApiAccessor, UsersApiAccessor>();
            services.AddScoped<IKiosksApiAccessor, KiosksApiAccessor>();
            services.AddScoped<IPluginsApiAccessor, PluginsApiAccessor>();
        }
    }
}
