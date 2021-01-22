using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MigsTech.Blizzard.BusinessLogic.Managers;
using MigsTech.Blizzard.BusinessLogic.Models;
using MigsTech.Blizzard.Data.Services;

namespace MigsTech.Blizzard.BusinessLogic
{
    /// <summary>
    /// <see cref="IServiceCollection"/> Extensions for Entitlement Business Logic.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all of the dependencies into the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        public static void AddMigsTechServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Auth0Configuration>(options => configuration.GetSection(Auth0Configuration.Auth0ConfigurationKey).Bind(options));

            var sp = services.BuildServiceProvider();

            var auth0Config = sp.GetService<IOptions<Auth0Configuration>>().Value;

            services.AddCorsPolicies();

            services.AddAuthenticationPolicies(auth0Config);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Migs Tech Blizzard API", Version = "v1" });

                // Only include Controllers and their Actions that have a defined group name
                c.DocInclusionPredicate((_, controller) => !string.IsNullOrWhiteSpace(controller.GroupName));

                // Rather than grouping Actions by their Controllers, we group Actions by the group name of the controller.
                // If two separate controller files have similar business logic, we can give the two controllers the same
                // group name and Swagger will group the Actions in those two controllers together.
                c.TagActionsBy(controller => new[] { controller.GroupName });

                // Include the xml comments generated at build time for each Controller/Action
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
            });

            services.AddSwaggerGenNewtonsoftSupport();

            services.AddSingleton<IWoWTokenManager, WoWTokenManager>();
            services.AddSingleton<IWoWTokenService, WoWTokenService>();
            services.AddSingleton<IHttpService, HttpService>();

            services.AddOAuth2ServiceClient(configuration);
        }

        private static void AddCorsPolicies(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: "corsPolicy",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
        }

        private static void AddAuthenticationPolicies(this IServiceCollection services, Auth0Configuration auth0Config)
        {
            //services.AddJwtAuthentication(new JwtAuthenticationOptions
            //{
            //    Audience = auth0Config.Audience,
            //    Domain = auth0Config.Domain,
            //    RequiredScopes = auth0Config.RequiredScopes
            //});

            //services.AddSwagger(new SwaggerOptions()
            //{
            //    Title = Configuration["Swagger:Title"],
            //    Description = Configuration["Swagger:Description"],
            //    AuthorizationUrl = Configuration["Auth0:AuthorizationUrl"],
            //    XmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml",
            //    RequiredScopes = requiredScopes,
            //    ApiVersionCount = 2,
            //    ExampleAssembly = Assembly.GetEntryAssembly()
            //});

            services
                    .AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(options =>
                    {
                        options.Authority = auth0Config.Domain;
                        options.Audience = auth0Config.Audience;
                    });
        }

        /// <summary>
        /// Registers all of the dependencies into the service collection
        /// for the OAuth2 service client.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        private static void AddOAuth2ServiceClient(this IServiceCollection services, IConfiguration configuration)
        {
            Action<HttpClient> httpClientConfig = (HttpClient client) =>
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

            services.AddHttpClient<IOAuth2Service, OAuth2Service>()
                .ConfigureHttpClient(httpClientConfig);
        }
    }
}
