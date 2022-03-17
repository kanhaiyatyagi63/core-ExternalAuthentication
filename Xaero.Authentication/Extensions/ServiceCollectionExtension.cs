using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xaero.Authentication.Data;
using Xaero.Authentication.Enumerations;
using Xaero.Authentication.Managers;
using Xaero.Authentication.Managers.Abstracts;
using Xaero.Authentication.Models;
using Xaero.Authentication.Repositories;
using Xaero.Authentication.Repositories.Abstracts;

namespace Xaero.Authentication.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ServiceCollectionInfrastructure(this IServiceCollection services, IConfiguration configuration) {
            services.AddDataContext(configuration);
            services.RegisterServices();
            services.RegisterAuthentication(configuration);
            return services;
        }
        private static IServiceCollection AddDataContext(this IServiceCollection services, IConfiguration configuration) {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
        private static IServiceCollection RegisterServices(this IServiceCollection services) {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services;
        }
        private static IServiceCollection RegisterAuthentication(this IServiceCollection services, IConfiguration configuration) {

            var oidcProviders = new OidcProviders();
            configuration.Bind("Oidc", oidcProviders);
            services.AddSingleton(oidcProviders);

            var builder = services.AddAuthentication(options =>
            {
                options.DefaultScheme = SocialAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = SocialAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(SocialAuthenticationDefaults.AuthenticationScheme);

            foreach (OidcProvider provider in oidcProviders.Providers)
            {
                switch (provider.Name)
                {
                    case OidcProviderType.Google:
                        builder.AddGoogle(options =>
                        {
                            options.SaveTokens = true;
                            options.ClientId = provider.ClientId;
                            options.ClientSecret = provider.ClientSecret;
                            options.Events.OnTicketReceived = (context) =>
                            {
                                Console.WriteLine(context.HttpContext.User);
                                return Task.CompletedTask;
                            };
                            options.Events.OnCreatingTicket = (context) =>
                            {
                                Console.WriteLine(context.Identity);
                                return Task.CompletedTask;
                            };
                        });
                        break;
                    case OidcProviderType.Facebook:
                        builder.AddFacebook(options =>
                        {
                            options.SaveTokens = true;
                            options.ClientId = provider.ClientId;
                            options.ClientSecret = provider.ClientSecret;
                            options.Events.OnTicketReceived = (context) =>
                            {
                                Console.WriteLine(context.HttpContext.User);
                                return Task.CompletedTask;
                            };
                            options.Events.OnCreatingTicket = (context) =>
                            {
                                Console.WriteLine(context.Identity);
                                return Task.CompletedTask;
                            };
                        });
                        break;
                }
            }
            return services;
        }
    }
}
