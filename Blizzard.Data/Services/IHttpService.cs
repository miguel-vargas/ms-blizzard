using System.Net.Http;
using System.Threading.Tasks;

namespace MigsTech.Blizzard.Data.Services
{
    /// <summary>
    /// Wrapper for HTTP Client.
    /// </summary>
    public interface IHttpService
    {
        /// <summary>
        /// The HTTP client.
        /// </summary>
        HttpClient HttpClient { get; set; }

        /// <summary>
        /// Get request from specified uri.
        /// </summary>
        /// <param name="uri">The URI to send the get request to.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="uri"/> is null or empty.</exception>
        Task<HttpResponseMessage> GetRequestAsync(string uri);
    }
}
