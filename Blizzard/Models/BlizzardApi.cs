namespace MigsTech.Blizzard.Models
{
    /// <summary>
    /// Credentials for accessing the Blizzard API
    /// </summary>
    public class BlizzardApi
    {
        /// <summary>
        /// The Auth Uri for accessing the Blizzard API
        /// </summary>
        public string AuthUri { get; }

        /// <summary>
        /// The Client Id for accessing the Blizzard API
        /// </summary>
        public string ClientId { get; }

        /// <summary>
        /// The Client Secret for accessing the Blizzard API
        /// </summary>
        public string ClientSecret { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlizzardApi"/> class.
        /// </summary>
        /// /// <param name="authUri">The Auth Uri.</param>
        /// <param name="clientId">The Client Id.</param>
        /// <param name="clientSecret">The Client Secret</param>
        public BlizzardApi(string authUri, string clientId, string clientSecret)
        {
            this.AuthUri = authUri;
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
        }
    }
}
