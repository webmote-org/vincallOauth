using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vincall.OauthService.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using IdentityServer4.EntityFramework;
using IdentityServer4.EntityFramework.Storage;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using IdentityModel;
using Vincall.OauthService.UserDB;
using Microsoft.IdentityModel.Logging;
using System.Net;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Vincall.Infrastructure;
using Vincall.OauthService.Middleware;
using IdentityServer4.Validation;

namespace Vincall.OauthService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

       
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddMemoryCache();
            services.AddEntityFrameworkSqlServer();

            var connectionString = Configuration.GetConnectionString("AuthDB");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });
            var x509Cert = new System.Security.Cryptography.X509Certificates.X509Certificate2("vincall.pfx");

            services.AddScoped<IProfileService, VincallUserProfileService>();
            services.AddScoped<IUserInfoRequestValidator, UserInfoRequestValidator>();
            services.AddIdentityServer(
                options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    options.InputLengthRestrictions.RedirectUri = 5000;
                })
                .AddVincallUsers(connectionString)                 
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                })
               .AddSigningCredential(x509Cert)
               .AddRedirectUriValidator<RedirectUriValidator>()
            ;
            services.AddCors(options =>
                options.AddPolicy("api", p =>
                {
                    p.SetIsOriginAllowed((host) => true);
                    p.AllowAnyHeader();
                    p.AllowAnyMethod();
                    p.AllowCredentials();
                    p.SetPreflightMaxAge(TimeSpan.FromSeconds(24 * 60 * 60));
                }));
           
            services.AddOidcStateDataFormatterCache("aad");
            services.AddScoped<ITokenRequestValidator, TokenRequestValidator>();

            services.AddSingleton<IXmlRepository, CustomXmlRepository>();
            string Name = ".AspNet.SharedCookie";
            services.AddDataProtection()
                .PersistKeysToDbContext<VincallDBContext>()
                .ProtectKeysWithCertificate(x509Cert)
                .SetApplicationName("vincall");

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme            )
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Cookie.Name = Name;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Lax;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.Domain = Configuration["Domain"]; //".vincall.net";
                    options.ExpireTimeSpan = TimeSpan.FromDays(10);
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };
                }); 
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            if (Configuration["InitDB"] == "true")
            {
                Console.WriteLine("Start initialize database...");
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                    var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                    context.Database.Migrate();
                    if (!context.Clients.Any())
                    {
                        foreach (var client in Configs.GetClients())
                        {
                            context.Clients.Add(client.ToEntity());
                        }
                        context.SaveChanges();
                    }

                    if (!context.IdentityResources.Any())
                    {
                        foreach (var resource in Configs.GetIdentityResources())
                        {
                            context.IdentityResources.Add(resource.ToEntity());
                        }
                        context.SaveChanges();
                    }

                    if (!context.ApiScopes.Any())
                    {
                        foreach (var resource in Configs.GetApiScopes())
                        {
                            context.ApiScopes.Add(resource.ToEntity());
                        }
                        context.SaveChanges();
                    }
                }
                Console.WriteLine("End initialize database!!!");
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitializeDatabase(app);
            app.Use(async (ctx, next) =>
            {
                var isInDocker = "true" == Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");
                if (isInDocker)
                {
                    ctx.Request.Scheme = "https";
                }
                await next();
            });
            app.UseCookiePolicy();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCors("api");
            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
