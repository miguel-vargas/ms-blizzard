using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MigsTech.Blizzard.Data.Models
{
    /// <summary>
    /// Regions available.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum WowRegion
    {
        /// <summary>
        /// North America.
        /// </summary>
        Us,

        /// <summary>
        /// European Union.
        /// </summary>
        Eu,

        /// <summary>
        /// China.
        /// </summary>
        Cn,

        /// <summary>
        /// Taiwan.
        /// </summary>
        Tw,

        /// <summary>
        /// Korea.
        /// </summary>
        Kr,
    }
}
