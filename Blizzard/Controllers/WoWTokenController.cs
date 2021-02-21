using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MigsTech.Blizzard.BusinessLogic.Managers;
using MigsTech.Blizzard.BusinessLogic.Models;

namespace MigsTech.Blizzard.Controllers
{
    /// <summary>
    /// The controller for getting WoW Tokens.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("~/wow")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "WoW Token")]
    public class WoWTokenController : ControllerBase
    {
        #region Fields and Properties
        private readonly IWoWTokenManager wowTokenManager;
        private readonly ILogger<WoWTokenController> logger;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WoWTokenController"/> class.
        /// </summary>
        /// <param name="wowTokenManager">The WoW Token Manager.</param>
        /// <param name="logger">The logger</param>
        public WoWTokenController(IWoWTokenManager wowTokenManager, ILogger<WoWTokenController> logger)
        {
            this.wowTokenManager = wowTokenManager;
            this.logger = logger;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the WoW Token for every region.
        /// </summary>
        /// <returns></returns>
        [HttpGet("tokens", Name = "Get All WoW Tokens")]
        public async Task<WoWTokenResponse> GetAllWoWTokens()
        {
            return await this.wowTokenManager.GetAllWoWTokens();
        }

        /// <summary>
        /// Gets the WoW Token for the specified region.
        /// </summary>
        /// <returns></returns>
        [HttpGet("tokens/{region}", Name = "Get WoW Token By Region")]
        public async Task<WoWTokenItem> GetTokenByRegion([FromRoute(Name = "region")][Required] WowRegion wowRegion)
        {
            return await this.wowTokenManager.GetWoWTokenByRegion(wowRegion);
        }
        #endregion
    }
}
