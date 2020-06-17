using System.Threading.Tasks;

namespace MigsTech.Blizzard.Data.Services
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
        Task<string> GetAuthToken();
    }
}
