using KioskStream.Core.Configurations;
using KioskStream.Core.Security.Authorization;
using KioskStream.Data;
using KioskStream.Mailing;
using KioskStream.Models;
using KioskStream.Web.Server.Extensions;
using KioskStream.Web.Server.Filters;
using KioskStream.Web.Server.Handlers;
using KioskStream.Web.Server.Managers;
using KioskStream.Web.Server.Managers.Interfaces;
using KioskStream.Web.Server.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Reflection;
using System.Text;

namespace KioskStream.Web.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options => { options.Filters.Add(new ModelStateValidationFilter()); });
            services.AddRazorPages();
            services.Configure<ApplicationConfiguration>(Configuration.GetSection(nameof(ApplicationConfiguration)));
            services.Configure<MailingServiceConfiguration>(Configuration.GetSection(nameof(MailingServiceConfiguration)));
            services.AddSingleton<FileContentProvider>();
            string migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            void DbContextOptionsBuilder(DbContextOptionsBuilder builder)
            {
                builder.UseSqlServer(Configuration.GetConnectionString("KioskStreamDatabaseConnection"));
            }

            services.AddDbContext<KioskStreamDbContext>(DbContextOptionsBuilder);
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddIdentity<User, IdentityRole<int>>(options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                })
                .AddEntityFrameworkStores<KioskStreamDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["ApplicationConfiguration:Authentication:Issuer"],
                        ValidAudience = Configuration["ApplicationConfiguration:Authentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["ApplicationConfiguration:Authentication:SecretKey"]))
                    };
                });

            services.AddAuthorization(options =>
            {
                //options.AddApplicationStaticPolicies();
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
            });

            //services.AddSingleton<IAuthorizationPolicyProvider, PolicyProvider>();
            
            //services.AddTransient<IAuthorizationHandler, SelfActionPermissionRequirementHandler>();
            services.AddScoped<IAuthenticationStateManager, AuthenticationStateManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IEmailProcessor, EmailProcessor>();
            services.AddScoped<IKioskManager, KioskManager>();
            services.AddScoped<IAccountManager, AccountManager>();
            services.AddScoped<IPluginManager, PluginManager>();
            services.AddScoped<IKioskPluginsManager, KioskPluginsManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                //endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });

            InitializeApplicationDatabase(app);
        }

        private void InitializeApplicationDatabase(IApplicationBuilder app)
        {
            return;
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            var scope = scopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        }
    }
}
