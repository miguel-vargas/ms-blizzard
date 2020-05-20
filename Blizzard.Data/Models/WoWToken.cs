using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MigsTech.Blizzard.Data.Models
{
    /// <summary>
    /// Represents a wow token.
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class WoWToken
    {
        #region Fields and Properties
        /// <summary>
        /// Gets or sets the data identifier.
        /// </summary>
        //[JsonProperty(PropertyName = "last_updated_timestamp")]
        public string LastUpdatedTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the revision number.
        /// </summary>
        public string Price { get; set; }
        #endregion
    }
}
