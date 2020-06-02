using System;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Logging;
using MigsTech.Blizzard.BusinessLogic.Managers;
using MigsTech.Blizzard.Controllers;
using MigsTech.Blizzard.Data.Models;
using Moq;
using Xunit;

namespace MigsTech.Blizzard.UnitTests.Controllers
{
    public class WoWTokenControllerTests
    {
        private readonly Mock<IWoWTokenManager> wowTokenManager;
        private readonly WoWTokenController controller;

        public WoWTokenControllerTests()
        {
            this.wowTokenManager = new Mock<IWoWTokenManager>();
            var loggerMock = new Mock<ILogger<WoWTokenController>>();
            this.controller = new WoWTokenController(this.wowTokenManager.Object, loggerMock.Object);
        }

        [Fact]
        public void WoWTokenController_GetAllWoWTokens_ReturnsTypeWoWTokenResponse()
        {
            // Arrange
            this.wowTokenManager.Setup(m => m.GetAllWoWTokens()).ReturnsAsync(new WoWTokenResponse(new []{ new WoWTokenItem(), }));

            // Act
            var response = this.controller.GetAllWoWTokens();

            // Assert
            Assert.IsAssignableFrom<WoWTokenResponse>(response.Result);
        }

        [Fact]
        public void WoWTokenController_GetAllWoWTokens_ReturnsWoWTokenTokenItem()
        {
            // Arrange
            const WowRegion region = WowRegion.Us;

            var expectedToken = new WoWTokenItem()
            {
                LastUpdatedTimestamp = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                Price = string.Empty,
                Region = region.ToString()
            };

            this.wowTokenManager.Setup(m => m.GetAllWoWTokens()).ReturnsAsync(new WoWTokenResponse(new[] { expectedToken, }));

            // Act
            var response = this.controller.GetAllWoWTokens();

            // Assert
            var actualToken = response.Result.WowTokens.ToList().First();
            Assert.Equal(expectedToken.LastUpdatedTimestamp, actualToken.LastUpdatedTimestamp);
            Assert.Equal(expectedToken.Price, actualToken.Price);
            Assert.Equal(expectedToken.Region, actualToken.Region);
        }

        [Fact]
        public void WoWTokenController_GetWoWTokenByRegion_ReturnsTypeWoWTokenItem()
        {
            // Arrange
            this.wowTokenManager.Setup(m => m.GetWoWTokenByRegion(It.IsAny<WowRegion>())).ReturnsAsync(new WoWTokenItem());

            // Act
            var response = this.controller.GetTokenByRegion(WowRegion.Us);

            // Assert
            Assert.IsAssignableFrom<WoWTokenItem>(response.Result);
        }

        [Fact]
        public void WoWTokenController_GetWoWTokenByRegion_ReturnsWoWTokenTokenItem()
        {
            // Arrange
            const WowRegion region = WowRegion.Us;

            var expectedToken = new WoWTokenItem()
            {
                LastUpdatedTimestamp = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                Price = string.Empty,
                Region = region.ToString()
            };

            this.wowTokenManager.Setup(m => m.GetWoWTokenByRegion(It.IsAny<WowRegion>())).ReturnsAsync(expectedToken);

            // Act
            var response = this.controller.GetTokenByRegion(region);

            // Assert
            Assert.Equal(expectedToken.LastUpdatedTimestamp, response.Result.LastUpdatedTimestamp);
            Assert.Equal(expectedToken.Price, response.Result.Price);
            Assert.Equal(expectedToken.Region, response.Result.Region);
        }
    }
}
