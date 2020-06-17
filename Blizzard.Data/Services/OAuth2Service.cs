using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace MigsTech.Blizzard.Data.Services
{
    /// <summary>
    /// A service for handling the oAuth2 flow, giving us an access token every time we want to interact with an API.
    /// </summary>
    public class OAuth2Service : IOAuth2Service
    {
        #region Fields and Properties
        internal const string BlizzardAuthUri = "https://us.battle.net/oauth/token";

        private readonly HttpClient client;
        private readonly ILogger<OAuth2Service> logger;

        private string token = null;
        private DateTime tokenExpiry = DateTime.Now;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2Service"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="logger">The logger</param>
        public OAuth2Service(HttpClient client, ILogger<OAuth2Service> logger)
        {
            this.client = client;
            this.logger = logger;
        }
        #endregion

        #region Methods
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

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"grant_type", "client_credentials"},
            });

            var response = await this.client.PostAsync(BlizzardAuthUri, content);

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

            return DateTime.Compare(DateTime.Now, tokenExpiry) < 0;
        } 
        #endregion
    }
}
