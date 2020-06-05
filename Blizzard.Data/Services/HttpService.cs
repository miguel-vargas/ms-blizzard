using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MigsTech.Blizzard.Data.Services
{
    /// <inheritdoc cref="IHttpService"/>
    public class HttpService : IHttpService, IDisposable
    {
        #region Fields and Properties
        /// <inheritdoc />
        public HttpClient HttpClient { get; set; }

        private readonly IOAuth2Service authService;
        private readonly ILogger<HttpService> logger;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpService"/> class.
        /// </summary>
        /// <param name="authService">The Auth Service.</param>
        /// <param name="logger">The logger</param>
        public HttpService(IOAuth2Service authService, ILogger<HttpService> logger)
        {
            this.HttpClient = new HttpClient();

            this.authService = authService;
            this.logger = logger;
        }
        #endregion

        #region Methods
        /// <inheritdoc />
        public async Task<HttpResponseMessage> GetRequestAsync(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException(nameof(uri));
            }

            await this.GetAndAppendAccessToken();

            var response = await this.HttpClient.GetAsync(uri);

            return response;
        }

        /// <summary>
        /// Get access token from the auth service and appends it to the HTTP request headers.
        /// </summary>
        /// <returns></returns>
        private async Task GetAndAppendAccessToken()
        {
            var accessToken = await this.authService.GetAuthToken();
            AppendToken(accessToken);
        }

        /// <summary>
        /// Appends the specified access token to the HTTP request headers.
        /// </summary>
        /// <param name="accessToken"></param>
        private void AppendToken(string accessToken)
        {
            this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            HttpClient?.Dispose();
        } 
        #endregion
    }
}
