using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MigsTech.Blizzard.Interfaces;
using Newtonsoft.Json.Linq;

namespace MigsTech.Blizzard.Services
{
    /// <summary>
    /// A service for handling the oAuth2 flow, giving us an access token every time we want to interact with an API.
    /// </summary>
    public class OAuth2Service : IOAuth2Service
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<OAuth2Service> logger;

        private string token = null;
        private DateTime? tokenExpiry = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2Service"/> class.
        /// </summary>
        /// <param name="configuration">The configuration</param>
        /// <param name="logger">The logger</param>
        public OAuth2Service(IConfiguration configuration, ILogger<OAuth2Service> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        /// <summary>
        /// Gets access token if current one is expired.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetToken()
        {
            if (IsTokenInvalid())
            {
                using var client = new HttpClient();

                var request = new HttpRequestMessage(HttpMethod.Post, new Uri(this.configuration["Blizzard:AuthUri"]))
                {
                    Content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        {"grant_type", "client_credentials"},
                    })
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                            $"{Encoding.UTF8.GetString(Convert.FromBase64String(this.configuration["Blizzard:ClientId"]))}:{Encoding.UTF8.GetString(Convert.FromBase64String(this.configuration["Blizzard:ClientSecret"]))}"
                        )
                    )
                );

                var response = await client.SendAsync(request);

                var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

                this.tokenExpiry = DateTime.Now.AddSeconds(payload.Value<double>("expires_in"));
                this.token = payload.Value<string>("access_token");
            }

            return await Task.FromResult(this.token);
        }

        /// <summary>
        /// Checks if current token is invalid.
        /// </summary>
        /// <returns></returns>
        public bool IsTokenInvalid()
        {
            if (token == null)
            {
                return true;
            }

            if (tokenExpiry == null)
            {
                return true;
            }

            return DateTime.Compare(DateTime.Now, tokenExpiry.Value) > 0;
        }
    }
}
