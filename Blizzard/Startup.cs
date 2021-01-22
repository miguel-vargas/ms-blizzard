using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
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
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver() { NamingStrategy = new CamelCaseNamingStrategy() };
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                });

            services.AddMigsTechServices(_configuration);
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

            app.UseCors("corsPolicy");

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        #endregion
    }
}
