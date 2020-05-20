using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MigsTech.Blizzard.BusinessLogic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MigsTech.Blizzard
{
    /// <summary>
    /// Application Startup
    /// </summary>
    public class Startup
    {
        #region Fields and Properties
        private readonly IConfiguration configuration;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        #endregion

        #region Methods
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver() { NamingStrategy = new SnakeCaseNamingStrategy() };
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                });

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

            services.AddTokenBusinessLogic(this.configuration);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The hosting environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Use static files so we can inject custom css for our Swagger UI
            app.UseStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Migs Tech Blizzard API");
                c.RoutePrefix = string.Empty;
                c.EnableDeepLinking();
                c.InjectStylesheet("/swagger-ui/BlizzardAPISwagger.css");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        } 
        #endregion
    }
}
