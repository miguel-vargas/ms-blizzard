using MigsTech.Blizzard.BusinessLogic.Services.Interfaces;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MigsTech.Blizzard.BusinessLogic.Handlers
{
    /// <summary>
    /// Handles Authorization for Blizzard's API
    /// </summary>
    public class AuthHandler : DelegatingHandler
    {
        private readonly IAuthService authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthHandler"/> class.
        /// </summary>
        /// <param name="authService"></param>
        public AuthHandler(IAuthService authService)
        {
            this.authService = authService;
        }

        /// <summary>
        /// Grab auth token and insert it to the headers of an http request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authToken = await authService.GetAuthTokenAsync();

            if (request.Headers.Contains("Authorization"))
            {
                request.Headers.Remove("Authorization");
            }

            request.Headers.Add("Authorization", $"Bearer {authToken}");
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
