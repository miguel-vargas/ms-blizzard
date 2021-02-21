namespace MigsTech.Blizzard.BusinessLogic.Models
{
    /// <summary>
    /// Represents settings to authenticate against Blizzard's API
    /// </summary>
    public class BlizzardOAuthSettings
    {
        internal const string BlizzardSettingsKey = "Blizzard";

        /// <summary>
        /// Blizzard's Auth Endpoint.
        /// </summary>
        public string BlizzardUri { get; set; }

        /// <summary>
        /// Client Id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Client Secret.
        /// </summary>
        public string ClientSecret { get; set; }
    }
}
