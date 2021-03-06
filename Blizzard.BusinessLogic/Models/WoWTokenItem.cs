﻿namespace MigsTech.Blizzard.BusinessLogic.Models
{
    /// <summary>
    /// Represents a wow token item.
    /// </summary>
    public class WoWTokenItem
    {
        #region Fields and Properties
        /// <summary>
        /// Gets or sets the data identifier.
        /// </summary>
        public string LastUpdatedTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the revision number.
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// Gets or sets the region of this wow token.
        /// </summary>
        public string Region { get; set; }
        #endregion
    }
}
