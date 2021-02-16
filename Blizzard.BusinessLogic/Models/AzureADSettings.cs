namespace MigsTech.Blizzard.BusinessLogic.Models
{
    /// <summary>
    /// Represents Auth0 settings model
    /// </summary>
    public class AzureADSettings
    {
        internal const string AzureADSettingsKey = "AzureAd";
        /// <summary>
        /// Azure AD Instance.
        /// </summary>
        public string Instance { get; set; }

        /// <summary>
        /// Azure AD Domain.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Azure AD Tenant Id.
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// Azure AD Client Id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Azure AD Scope.
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// Azure AD Admin Consent Name.
        /// </summary>
        public string AdminConsentName { get; set; }
    }
}
