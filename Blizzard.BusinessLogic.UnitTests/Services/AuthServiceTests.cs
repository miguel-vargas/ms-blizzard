using Microsoft.Extensions.Logging.Abstractions;
using MigsTech.Blizzard.BusinessLogic.Services;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace MigsTech.Blizzard.BusinessLogic.UnitTests.Services
{
    public class AuthServiceTests
    {
        #region Fields and Properties
        internal const string DefaultAuthResponse = @"{ 'expires_in': 86399, 'access_token': '12345' }";

        private readonly MockHttpMessageHandler _handler;
        private readonly AuthService _service;
        #endregion

        #region Constructors
        public AuthServiceTests()
        {
            _handler = new MockHttpMessageHandler();

            _service = new AuthService(new HttpClient(_handler), NullLogger<AuthService>.Instance);
        }
        #endregion

        #region Tests
        [Fact]
        public void OAuth2Service_GetAuthToken_ReturnsTypeString()
        {
            // Arrange
            _handler
                .Expect(HttpMethod.Post, AuthService.BlizzardAuthEndpoint)
                .Respond(HttpStatusCode.OK, "application/json", DefaultAuthResponse);

            // Act
            var response = _service.GetAuthTokenAsync();

            // Assert
            Assert.IsAssignableFrom<string>(response.Result);
            _handler.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task OAuth2Service_GetAuthToken_OnMultipleCallsWithinValidityWindow_CallsHttpOnce()
        {
            // Arrange
            var request = _handler
                .When(HttpMethod.Post, AuthService.BlizzardAuthEndpoint)
                .Respond(HttpStatusCode.OK, "application/json", DefaultAuthResponse);

            // Act
            await _service.GetAuthTokenAsync();
            await _service.GetAuthTokenAsync();

            // Assert
            Assert.Equal(1, _handler.GetMatchCount(request));
        }

        [Fact]
        public async Task OAuth2Service_GetAuthToken_OnMultipleCallsWithinValidityWindow_ReturnsPreviousToken()
        {
            // Arrange
            const string expectedToken = "12345";

            var request = _handler
                .Expect(HttpMethod.Post, AuthService.BlizzardAuthEndpoint)
                .Respond(HttpStatusCode.OK, "application/json", DefaultAuthResponse);

            await _service.GetAuthTokenAsync();

            // Act
            var response = await _service.GetAuthTokenAsync();

            // Assert
            Assert.Equal(expectedToken, response);
        }

        [Fact]
        public async Task OAuth2Service_GetAuthToken_OnMultipleCallsOutsideValidityWindow_CallsHttpAgain()
        {
            // Arrange
            const string expectedToken = "12345";
            const string tokenExpiredAuthResponse = @"{
                'expires_in': 0,
                'access_token': '45678',
            }";

            _handler
                .Expect(HttpMethod.Post, AuthService.BlizzardAuthEndpoint)
                .Respond(HttpStatusCode.OK, "application/json", tokenExpiredAuthResponse);

            await _service.GetAuthTokenAsync();

            _handler
                .Expect(HttpMethod.Post, AuthService.BlizzardAuthEndpoint)
                .Respond(HttpStatusCode.OK, "application/json", DefaultAuthResponse);

            // Act
            var response = await _service.GetAuthTokenAsync();

            // Assert
            Assert.Equal(expectedToken, response);
            _handler.VerifyNoOutstandingExpectation();
        }
        #endregion
    }
}
