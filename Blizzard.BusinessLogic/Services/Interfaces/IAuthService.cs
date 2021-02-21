using System.Threading.Tasks;

namespace MigsTech.Blizzard.BusinessLogic.Services.Interfaces
{
    /// <summary>
    /// A service for handling the oAuth2 flow, giving us an access token every time we want to interact with an API.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Gets access token if current one is expired.
        /// </summary>
        /// <returns></returns>
        Task<string> GetAuthTokenAsync();
    }
}
