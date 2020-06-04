using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace MigsTech.Blizzard.Data.Services
{
    /// <summary>
    /// A service for handling the oAuth2 flow, giving us an access token every time we want to interact with an API.
    /// </summary>
    public class OAuth2Service : IOAuth2Service
    {
        private readonly HttpClient client;
        private readonly IConfiguration configuration;
        private readonly ILogger<OAuth2Service> logger;

        private string token = null;
        private DateTime? tokenExpiry = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2Service"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="configuration">The configuration</param>
        /// <param name="logger">The logger</param>
        public OAuth2Service(HttpClient client, IConfiguration configuration, ILogger<OAuth2Service> logger)
        {
            this.client = client;
            this.configuration = configuration;
            this.logger = logger;
        }

        /// <summary>
        /// Gets access token if current one is expired.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAuthToken()
        {
            if (this.IsTokenValid())
            {
                return this.token;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(this.configuration["Blizzard:AuthUri"]))
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"grant_type", "client_credentials"},
                })
            };

            var response = await this.client.SendAsync(request);

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            this.tokenExpiry = DateTime.Now.AddSeconds(payload.Value<double>("expires_in"));
            this.token = payload.Value<string>("access_token");

            return this.token;
        }

        /// <summary>
        /// Checks if current token is valid.
        /// </summary>
        /// <returns></returns>
        private bool IsTokenValid()
        {
            if (token == null)
            {
                return false;
            }

            if (tokenExpiry == null)
            {
                return false;
            }

            return DateTime.Compare(DateTime.Now, tokenExpiry.Value) < 0;
        }
    }
}
