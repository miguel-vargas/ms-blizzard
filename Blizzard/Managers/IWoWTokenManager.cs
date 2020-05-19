using System.Threading.Tasks;

namespace MigsTech.Blizzard.Managers
{
    /// <summary>
    /// An interface for the WoW Token manager.
    /// </summary>
    public interface IWoWTokenManager
    {
        /// <summary>
        /// Gets the WoW Token for the specified region.
        /// </summary>
        /// <param name="region">The region.</param>
        /// <returns></returns>
        Task<string> GetWoWTokenByRegion(string region);
    }
}
