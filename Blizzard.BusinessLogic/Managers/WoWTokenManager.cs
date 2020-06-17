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
        private readonly IWoWTokenService wowTokenService;
        private readonly ILogger<WoWTokenManager> logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WoWTokenManager"/> class.
        /// </summary>
        /// <param name="wowTokenService">The WoW Token Service.</param>
        /// <param name="logger">The logger</param>
        public WoWTokenManager(IWoWTokenService wowTokenService, ILogger<WoWTokenManager> logger)
        {
            this.wowTokenService = wowTokenService;
            this.logger = logger;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the WoW Token for the specified region.
        /// </summary>
        /// <returns></returns>
        public async Task<WoWTokenResponse> GetAllWoWTokens()
        {
            return await this.wowTokenService.GetAllWoWTokens();
        }

        /// <summary>
        /// Gets the WoW Token for the specified region.
        /// </summary>
        /// <param name="wowRegion">The region.</param>
        /// <returns></returns>
        public async Task<WoWTokenItem> GetWoWTokenByRegion(WowRegion wowRegion)
        {
            return await this.wowTokenService.GetWoWTokenByRegion(wowRegion);
        }
        #endregion
    }
}
