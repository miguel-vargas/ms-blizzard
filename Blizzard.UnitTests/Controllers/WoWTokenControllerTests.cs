using Microsoft.Extensions.Logging;
using MigsTech.Blizzard.Controllers;
using Moq;
using Xunit;

namespace MigsTech.Blizzard.UnitTests.Controllers
{
    public class WoWTokenControllerTests
    {
        private readonly WoWTokenController controller;

        public WoWTokenControllerTests()
        {
            var loggerMock = new Mock<ILogger<WoWTokenController>>();
            this.controller = new WoWTokenController(loggerMock.Object);
        }

        [Fact]
        public void WoWTokenController_Get_ReturnsTypeString()
        {
            // Arrange
            const string testRegion = "test";

            // Act
            var response = this.controller.GetTokenByRegion(testRegion);

            // Assert
            Assert.IsAssignableFrom<string>(response.Result);
        }

        [Fact]
        public void WoWTokenController_Get_ReturnsGivenString()
        {
            // Arrange
            const string testRegion = "test";

            // Act
            var response = this.controller.GetTokenByRegion(testRegion);

            // Assert
            Assert.Equal(testRegion, response.Result);
        }
    }
}
