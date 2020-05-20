using System.Threading.Tasks;
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
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WoWTokenManager"/> class.
        /// </summary>
        public WoWTokenManager(IWoWTokenService wowTokenService)
        {
            this.wowTokenService = wowTokenService;
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
