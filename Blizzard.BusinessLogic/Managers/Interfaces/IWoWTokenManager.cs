﻿using MigsTech.Blizzard.BusinessLogic.Models;
using System.Threading.Tasks;

namespace MigsTech.Blizzard.BusinessLogic.Managers.Interfaces
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
        Task<WoWTokenResponse> GetAllWoWTokens();

        /// <summary>
        /// Gets the WoW Token for the specified region.
        /// </summary>
        /// <param name="wowRegion">The region.</param>
        /// <returns></returns>
        Task<WoWTokenItem> GetWoWTokenByRegion(WowRegion wowRegion);
    }
}
