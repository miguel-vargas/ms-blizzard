using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MigsTech.Blizzard.Data.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MigsTech.Blizzard.Data.Services
{
    /// <summary>
    /// Represents the service which takes care of getting wow tokens from the Blizzard API.
    /// </summary>
    public class WoWTokenService : IWoWTokenService
    {
        #region Fields and Properties
        internal const string GetWoWTokenByRegionUriPattern = "https://{0}.api.blizzard.com/data/wow/token/index";
        internal const string NamespacePattern = "namespace=dynamic-{0}";

        private readonly HttpClient client;
        private readonly IOAuth2Service authService;
        private readonly ILogger<WoWTokenService> logger;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WoWTokenService"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="authService">The auth service.</param>
        /// <param name="logger">The logger</param>
        public WoWTokenService(HttpClient client, IOAuth2Service authService, ILogger<WoWTokenService> logger)
        {
            this.client = client;
            this.authService = authService;
            this.logger = logger;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the WoW Token for every region.
        /// </summary>
        /// <returns></returns>
        // TODO: Change return type to an object with a list of WoW token items
        public async Task<WoWToken> GetAllWoWTokens()
        {
            // TODO: Set up auth in a repeatable way, maybe leverage the IsTokenValid call
            var token = await this.authService.GetToken();

            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // TODO: Stick this in a foreach region block
            var uriBuilder = new UriBuilder(string.Format(GetWoWTokenByRegionUriPattern, "us"))
            {
                Query = string.Format(NamespacePattern, "us")
            };

            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(uriBuilder.ToString()));

            var response = await client.SendAsync(request);

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            var price = payload.Value<string>("price");

            var wowToken = Newtonsoft.Json.JsonConvert.DeserializeObject<WoWToken>(await response.Content.ReadAsStringAsync());

            return wowToken;
        }

        /// <summary>
        /// Gets the WoW Token for the specified region.
        /// </summary>
        /// <param name="region">The region.</param>
        /// <returns></returns>
        // TODO: Change return type to WoW token items
        public async Task<WoWToken> GetWoWTokenByRegion(string region)
        {
            //await this.AuthenticateWithBlizzard();

            // TODO: Set up auth in a repeatable way, maybe leverage the IsTokenValid call
            var token = await this.authService.GetToken();

            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var uriBuilder = new UriBuilder(string.Format(GetWoWTokenByRegionUriPattern, region))
            {
                Query = string.Format(NamespacePattern, region)
            };

            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(uriBuilder.ToString()));

            var response = await client.SendAsync(request);

            var wowToken = JsonConvert.DeserializeObject<WoWToken>(await response.Content.ReadAsStringAsync());

            return wowToken;
        }

        //private async Task AuthenticateWithBlizzard()
        //{
            
        //}
        #endregion
    }
}
