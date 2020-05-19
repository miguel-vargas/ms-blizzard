using System;
using System.Threading.Tasks;
using MigsTech.Blizzard.Interfaces;

namespace MigsTech.Blizzard.Managers
{
    /// <summary>
    /// The WoW Token Manager
    /// </summary>
    public class WoWTokenManager : IWoWTokenManager
    {
        #region Fields and Properties
        private readonly IOAuth2Service authService;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WoWTokenManager"/> class.
        /// </summary>
        /// <param name="authService">The auth service</param>
        public WoWTokenManager(IOAuth2Service authService)
        {
            this.authService = authService;
        }
        #endregion

        /// <summary>
        /// Gets the WoW Token for the specified region.
        /// </summary>
        /// <param name="region">The region.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<string> GetWoWTokenByRegion(string region)
        {
            var token = await this.authService.GetToken();
            throw new NotImplementedException();
        }
    }
}
