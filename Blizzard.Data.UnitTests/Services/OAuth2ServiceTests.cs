using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MigsTech.Blizzard.Data.Services;
using Moq;
using RichardSzalay.MockHttp;
using Xunit;

namespace MigsTech.Blizzard.Data.UnitTests.Services
{
    public class OAuth2ServiceTests
    {
        #region Fields and Properties
        internal const string DefaultAuthResponse = @"{ 'expires_in': 86399, 'access_token': '12345' }";

        private readonly MockHttpMessageHandler handler;
        private readonly Mock<ILogger<OAuth2Service>> logger;
        private readonly OAuth2Service service; 
        #endregion

        #region Constructors
        public OAuth2ServiceTests()
        {
            this.handler = new MockHttpMessageHandler();
            this.logger = new Mock<ILogger<OAuth2Service>>();

            this.service = new OAuth2Service(new HttpClient(this.handler), logger.Object);
        } 
        #endregion

        #region Tests
        [Fact]
        public void OAuth2Service_GetAuthToken_ReturnsTypeString()
        {
            // Arrange
            this.handler
                .Expect(HttpMethod.Post, OAuth2Service.BlizzardAuthUri)
                .Respond(HttpStatusCode.OK, "application/json", DefaultAuthResponse);

            // Act
            var response = this.service.GetAuthToken();

            // Assert
            Assert.IsAssignableFrom<string>(response.Result);
            this.handler.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task OAuth2Service_GetAuthToken_OnMultipleCallsWithinValidityWindow_CallsHttpOnce()
        {
            // Arrange
            var request = this.handler
                .When(HttpMethod.Post, OAuth2Service.BlizzardAuthUri)
                .Respond(HttpStatusCode.OK, "application/json", DefaultAuthResponse);

            await this.service.GetAuthToken();

            // Act
            var response = await this.service.GetAuthToken();

            // Assert
            Assert.Equal(1, this.handler.GetMatchCount(request));
        }

        [Fact]
        public async Task OAuth2Service_GetAuthToken_OnMultipleCallsWithinValidityWindow_ReturnsPreviousToken()
        {
            // Arrange
            const string expectedToken = "12345";

            var request = this.handler
                .Expect(HttpMethod.Post, OAuth2Service.BlizzardAuthUri)
                .Respond(HttpStatusCode.OK, "application/json", DefaultAuthResponse);

            await this.service.GetAuthToken();

            // Act
            var response = await this.service.GetAuthToken();

            // Assert
            Assert.Equal(1, this.handler.GetMatchCount(request));
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

            this.handler
                .Expect(HttpMethod.Post, OAuth2Service.BlizzardAuthUri)
                .Respond(HttpStatusCode.OK, "application/json", tokenExpiredAuthResponse);

            await this.service.GetAuthToken();

            this.handler
                .Expect(HttpMethod.Post, OAuth2Service.BlizzardAuthUri)
                .Respond(HttpStatusCode.OK, "application/json", DefaultAuthResponse);

            // Act
            var response = await this.service.GetAuthToken();

            // Assert
            Assert.Equal(expectedToken, response);
            this.handler.VerifyNoOutstandingExpectation();
        } 
        #endregion
    }
}
