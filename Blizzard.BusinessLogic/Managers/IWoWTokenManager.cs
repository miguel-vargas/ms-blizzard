using System.Threading.Tasks;
using MigsTech.Blizzard.Data.Models;

namespace MigsTech.Blizzard.BusinessLogic.Managers
{
    /// <summary>
    /// An interface for the WoW Token manager.
    /// </summary>
    public interface IWoWTokenManager
    {
        /// <summary>
        /// Gets the WoW Token for the specified region.
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
