using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MigsTech.Blizzard.Data.Models;
using MigsTech.Blizzard.Data.Services;

namespace MigsTech.Blizzard.BusinessLogic.Managers
{
    /// <summary>
    /// The WoW Token Manager
    /// </summary>
    public class WoWTokenManager : IWoWTokenManager
    {
        #region Fields and Properties
        private readonly ILogger _logger;
        private readonly IWoWTokenService _wowTokenService;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WoWTokenManager"/> class.
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="wowTokenService">The WoW Token Service.</param>
        public WoWTokenManager(
            ILogger<WoWTokenManager> logger,
            IWoWTokenService wowTokenService)
        {
            _logger = logger;
            _wowTokenService = wowTokenService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the WoW Token for the specified region.
        /// </summary>
        /// <returns></returns>
        public async Task<WoWTokenResponse> GetAllWoWTokens()
        {
            _logger.LogInformation($"Retrieving WoW Token data from all regions");
            return await _wowTokenService.GetAllWoWTokens();
        }

        /// <summary>
        /// Gets the WoW Token for the specified region.
        /// </summary>
        /// <param name="wowRegion">The region.</param>
        /// <returns></returns>
        public async Task<WoWTokenItem> GetWoWTokenByRegion(WowRegion wowRegion)
        {
            _logger.LogInformation($"Retrieving WoW Token data from {wowRegion}");
            return await _wowTokenService.GetWoWTokenByRegion(wowRegion);
        }
        #endregion
    }
}
