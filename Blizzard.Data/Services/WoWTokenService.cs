using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MigsTech.Blizzard.Data.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

        private readonly IHttpService httpService;
        private readonly ILogger<WoWTokenService> logger;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WoWTokenService"/> class.
        /// </summary>
        /// <param name="httpService">The HTTP Service.</param>
        /// <param name="logger">The logger</param>
        public WoWTokenService(IHttpService httpService, ILogger<WoWTokenService> logger)
        {
            this.httpService = httpService;
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
            var deserializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };

            var wowTokens = new List<WoWTokenItem>();

            var wowRegions = Enum.GetValues(typeof(WowRegion));

            foreach (var wowRegion in wowRegions)
            {
                var region = Enum.GetName(typeof(WowRegion), wowRegion)?.ToLower();

                var uri = BuildUriStringWithRegionQuery(region);

                var response = await httpService.GetRequestAsync(uri);

                var wowTokenItem = JsonConvert.DeserializeObject<WoWTokenItem>(await response.Content.ReadAsStringAsync(), deserializerSettings);

                if (wowTokenItem != null)
                {
                    wowTokenItem.Region = region;
                }

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
            var deserializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };

            var region = Enum.GetName(typeof(WowRegion), wowRegion)?.ToLower();

            var uri = BuildUriStringWithRegionQuery(region);

            var response = await httpService.GetRequestAsync(uri);

            var wowTokenItem = JsonConvert.DeserializeObject<WoWTokenItem>(await response.Content.ReadAsStringAsync(), deserializerSettings);

            if (wowTokenItem != null)
            {
                wowTokenItem.Region = region;
            }

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
