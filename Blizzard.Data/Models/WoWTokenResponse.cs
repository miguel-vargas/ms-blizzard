using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MigsTech.Blizzard.Data.Models
{
    /// <summary>
    /// Represents a wow token response containing one or more wow token items.
    /// </summary>
    public class WoWTokenResponse
    {
        #region Fields and Properties
        /// <summary>
        /// Gets or sets the wow token collection.
        /// </summary>
        public IEnumerable<WoWTokenItem> WowTokens { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WoWTokenResponse"/> class.
        /// </summary>
        /// <param name="wowTokens">The wow tokens.</param>
        public WoWTokenResponse(IEnumerable<WoWTokenItem> wowTokens)
        {
            this.WowTokens = wowTokens;
        }
        #endregion
    }
}

