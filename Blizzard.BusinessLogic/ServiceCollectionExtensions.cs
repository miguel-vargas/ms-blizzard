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
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MigsTech.Blizzard.BusinessLogic.Handlers;
using MigsTech.Blizzard.BusinessLogic.Managers;
using MigsTech.Blizzard.BusinessLogic.Managers.Interfaces;
using MigsTech.Blizzard.BusinessLogic.Models;
using MigsTech.Blizzard.BusinessLogic.Services;
using MigsTech.Blizzard.BusinessLogic.Services.Interfaces;

namespace MigsTech.Blizzard.BusinessLogic
{
    /// <summary>
    /// <see cref="IServiceCollection"/> Extensions for Entitlement Business Logic.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        internal const string ScopeNamePattern = "api://{0}/{1}";
        internal const string AuthorizeUrlPattern = "{0}{1}/oauth2/v2.0/authorize";
        internal const string TokenUrlPattern = "{0}{1}/oauth2/v2.0/token";
        internal const string SecurityDefinitionName = "oauth2";

        /// <summary>
        /// Registers all of the dependencies into the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        public static void AddMigsTechServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureADSettings>(options => configuration.GetSection(AzureADSettings.AzureADSettingsKey).Bind(options));
            services.Configure<BlizzardOAuthSettings>(options => configuration.GetSection(BlizzardOAuthSettings.BlizzardSettingsKey).Bind(options));

            var sp = services.BuildServiceProvider();

            var azureAdSettings = sp.GetService<IOptions<AzureADSettings>>().Value;
            var blizzardOAuthSettings = sp.GetService<IOptions<BlizzardOAuthSettings>>().Value;

            services.AddCorsPolicies();

            services.AddSwagger(azureAdSettings);

            services.AddAuthenticationPolicies(azureAdSettings);

            services.AddSwaggerGenNewtonsoftSupport();

            services.AddScoped<IWoWTokenManager, WoWTokenManager>();

            services.AddServiceClients(blizzardOAuthSettings);
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

        private static void AddSwagger(this IServiceCollection services, AzureADSettings azureAdSettings)
        {
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

                c.AddSecurityDefinition(SecurityDefinitionName, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.OAuth2,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Azure AD Auth",
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            Scopes = new Dictionary<string, string>
                            {
                                { string.Format(ScopeNamePattern, azureAdSettings.ClientId, azureAdSettings.Scope), azureAdSettings.AdminConsentName }
                            },
                            AuthorizationUrl = new Uri(string.Format(AuthorizeUrlPattern, azureAdSettings.Instance, azureAdSettings.TenantId)),
                            TokenUrl = new Uri(string.Format(TokenUrlPattern, azureAdSettings.Instance, azureAdSettings.TenantId))
                        }
                    }
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = SecurityDefinitionName
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        private static void AddAuthenticationPolicies(this IServiceCollection services, AzureADSettings azureAdSettings)
        {
            services
                .AddAuthentication(sharedOptions =>
                {
                    sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    // Authority will be Your AzureAd Instance and Tenant Id
                    options.Authority = $"{azureAdSettings.Instance}{azureAdSettings.TenantId}/v2.0";

                    // The valid audiences are both the Client ID(options.Audience) and api://{ClientID}
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudiences = new string[] { azureAdSettings.ClientId, $"api://{azureAdSettings.ClientId}" }
                    };
                });
        }

        /// <summary>
        /// Registers all services required to use Blizzard's OAuth.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="blizzardSettings"></param>
        /// <returns></returns>
        private static void AddServiceClients(this IServiceCollection services, BlizzardOAuthSettings blizzardSettings)
        {
            Action<HttpClient> httpClientConfig = (HttpClient client) =>
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(Encoding.ASCII.GetBytes($"{blizzardSettings.ClientId}:{blizzardSettings.ClientSecret}"))
                );

                client.BaseAddress = new Uri(blizzardSettings.BlizzardUri);
            };

            services.AddHttpClient<IAuthService, AuthService>()
                .ConfigureHttpClient(httpClientConfig);

            services.AddScoped<AuthHandler>();

            services.AddHttpClient<IWoWTokenService, WoWTokenService>()
                .AddHttpMessageHandler<AuthHandler>();
        }
    }
}
