using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MigsTech.Blizzard.Controllers
{
    /// <summary>
    /// The controller for getting WoW Tokens.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "WoW Token")]
    public class WoWTokenController : ControllerBase
    {
        private readonly ILogger<WoWTokenController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WoWTokenController"/> class.
        /// </summary>
        /// <param name="logger">The logger</param>
        public WoWTokenController(ILogger<WoWTokenController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Gets the WoW Token for the specified region.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{region}", Name = "Get WoW Token")]
        public async Task<string> GetTokenByRegion([FromRoute][Required] string region)
        {
            return await Task.FromResult(region);
        }
    }
}
