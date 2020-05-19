using System.Threading.Tasks;

namespace MigsTech.Blizzard.Interfaces
{
    /// <summary>
    /// A service for handling the oAuth2 flow, giving us an access token every time we want to interact with an API.
    /// </summary>
    public interface IOAuth2Service
    {
        /// <summary>
        /// Gets access token if current one is expired.
        /// </summary>
        /// <returns></returns>
        Task<string> GetToken();

        /// <summary>
        /// Checks if current token is valid.
        /// </summary>
        /// <returns></returns>
        bool IsTokenValid();
    }
}
