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
        public async Task<WoWToken> GetAllWoWTokens()
        {
            var wowTokens = await this.wowTokenService.GetAllWoWTokens();

            return wowTokens;
        }

        /// <summary>
        /// Gets the WoW Token for the specified region.
        /// </summary>
        /// <param name="region">The region.</param>
        /// <returns></returns>
        public async Task<WoWToken> GetWoWTokenByRegion(string region)
        {
            return await this.wowTokenService.GetWoWTokenByRegion(region);
        }
        #endregion
    }
}
