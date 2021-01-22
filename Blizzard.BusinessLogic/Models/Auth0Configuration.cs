namespace MigsTech.Blizzard.BusinessLogic.Models
{
    /// <summary>
    /// Represents Auth0 settings model
    /// </summary>
    public class Auth0Configuration
    {
        internal const string Auth0ConfigurationKey = "Auth0";
        /// <summary>
        /// Auth0 Domain.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Auth0 Audience.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Auth0 Authorization Url.
        /// </summary>
        public string AuthorizationUrl { get; set; }

        /// <summary>
        /// Auth0 Required Scopes.
        /// </summary>
        public string[] RequiredScopes { get; set; }
    }
}
