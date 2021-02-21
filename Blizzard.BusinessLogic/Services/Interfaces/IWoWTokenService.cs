using MigsTech.Blizzard.BusinessLogic.Models;
using System.Threading.Tasks;

namespace MigsTech.Blizzard.BusinessLogic.Services.Interfaces
{
    /// <summary>
    /// Represents the service which takes care of getting wow tokens from the Blizzard API.
    /// </summary>
    public interface IWoWTokenService
    {
        /// <summary>
        /// Gets the WoW Token for every region.
        /// </summary>
        /// <returns></returns>
        Task<WoWTokenResponse> GetAllWoWTokens();

        /// <summary>
        /// Gets the WoW Token for the specified region.
        /// </summary>
        /// <param name="wowRegion">The region.</param>
        /// <returns></returns>
        Task<WoWTokenItem> GetWoWTokenByRegion(WowRegion wowRegion);
    }
}
