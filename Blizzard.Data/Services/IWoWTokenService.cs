using System.Threading.Tasks;
using MigsTech.Blizzard.Data.Models;

namespace MigsTech.Blizzard.Data.Services
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
        Task<WoWToken> GetAllWoWTokens();

        /// <summary>
        /// Gets the WoW Token for the specified region.
        /// </summary>
        /// <param name="region">The region.</param>
        /// <returns></returns>
        Task<WoWToken> GetWoWTokenByRegion(string region);
    }
}
