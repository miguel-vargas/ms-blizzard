using System;
using System.Collections.Generic;
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
        internal const string ChineseUriPattern = "https://gateway.battlenet.com.{0}/data/wow/token/index";
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
        public async Task<WoWTokenResponse> GetAllWoWTokens()
        {
            var token = await this.authService.GetToken();

            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var wowTokens = new List<WoWTokenItem>();

            foreach (var wowRegion in Enum.GetValues(typeof(WowRegion)))
            {
                var region = Enum.GetName(typeof(WowRegion), wowRegion)?.ToLower();

                var uri = BuildUriStringWithRegionQuery(region);

                var request = new HttpRequestMessage(HttpMethod.Get, new Uri(uri));

                var response = await client.SendAsync(request);

                var wowTokenItem = JsonConvert.DeserializeObject<WoWTokenItem>(await response.Content.ReadAsStringAsync());

                wowTokenItem.Region = region;

                wowTokens.Add(wowTokenItem);
            }

            return new WoWTokenResponse(wowTokens);
        }

        /// <summary>
        /// Gets the WoW Token for the specified region.
        /// </summary>
        /// <param name="wowRegion">The region.</param>
        /// <returns></returns>
        public async Task<WoWTokenItem> GetWoWTokenByRegion(WowRegion wowRegion)
        {
            var token = await this.authService.GetToken();

            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var region = Enum.GetName(typeof(WowRegion), wowRegion)?.ToLower();

            var uri = BuildUriStringWithRegionQuery(region);

            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(uri));

            var response = await client.SendAsync(request);

            var wowTokenItem = JsonConvert.DeserializeObject<WoWTokenItem>(await response.Content.ReadAsStringAsync());

            wowTokenItem.Region = region;

            return wowTokenItem;
        }

        private static string BuildUriStringWithRegionQuery(string region)
        {
            return new UriBuilder(FormatUriByRegion(region))
            {
                Query = string.Format(NamespacePattern, region)
            }.ToString();
        }

        private static string FormatUriByRegion(string region)
        {
            // Handle logic for Chinese servers
            return region == Enum.GetName(typeof(WowRegion), WowRegion.Cn)?.ToLower() ? string.Format(ChineseUriPattern, region) : string.Format(GetWoWTokenByRegionUriPattern, region);
        }
        #endregion
    }
}
