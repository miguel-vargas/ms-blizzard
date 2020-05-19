using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MigsTech.Blizzard.Interfaces;
using MigsTech.Blizzard.Services;

namespace MigsTech.Blizzard
{
    /// <summary>
    /// <see cref="IServiceCollection"/> Extensions for Entitlement Business Logic.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all of the dependencies into the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        public static void AddTokenBusinessLogic(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOAuth2ServiceClient(configuration);
        }

        /// <summary>
        /// Registers all of the dependencies into the service collection
        /// for the OAuth2 service client.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        public static void AddOAuth2ServiceClient(this IServiceCollection services, IConfiguration configuration)
        {
            Action<HttpClient> configure = (HttpClient client) =>
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                            $"{Encoding.UTF8.GetString(Convert.FromBase64String(configuration["Blizzard:ClientId"]))}:{Encoding.UTF8.GetString(Convert.FromBase64String(configuration["Blizzard:ClientSecret"]))}"
                        )
                    )
                );
            };

            services.AddHttpClient<IOAuth2Service, OAuth2Service>(configure);
        }
    }
}
