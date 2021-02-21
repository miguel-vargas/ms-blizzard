using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MigsTech.Blizzard.BusinessLogic.Models;
using MigsTech.Blizzard.BusinessLogic.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MigsTech.Blizzard.BusinessLogic.Services
{
    /// <summary>
    /// A service for handling the oAuth2 flow, giving us an access token every time we want to interact with an API.
    /// </summary>
    public class AuthService : IAuthService
    {
        #region Fields and Properties
        internal const string BlizzardAuthEndpoint = "oauth/token";

        private readonly HttpClient _client;
        private readonly ILogger _logger;

        private string _token = null;
        private DateTime _tokenExpiry = DateTime.Now;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="logger">The logger</param>
        public AuthService(
            HttpClient client,
            ILogger<AuthService> logger)
        {
            _client = client;
            _logger = logger;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets access token if current one is expired.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAuthTokenAsync()
        {
            if (IsTokenValid())
            {
                return _token;
            }

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"grant_type", "client_credentials"},
            });

            var response = await _client.PostAsync(BlizzardAuthEndpoint, content);

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            _tokenExpiry = DateTime.Now.AddSeconds(payload.Value<double>("expires_in"));
            _token = payload.Value<string>("access_token");

            return _token;
        }

        /// <summary>
        /// Checks if current token is valid.
        /// </summary>
        /// <returns></returns>
        private bool IsTokenValid()
        {
            if (_token == null)
            {
                return false;
            }

            return DateTime.Compare(DateTime.Now, _tokenExpiry) < 0;
        }
        #endregion
    }
}
