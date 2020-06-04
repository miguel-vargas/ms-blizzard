using System;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Logging;
using MigsTech.Blizzard.BusinessLogic.Managers;
using MigsTech.Blizzard.Data.Models;
using MigsTech.Blizzard.Data.Services;
using Moq;
using Xunit;

namespace MigsTech.Blizzard.BusinessLogic.UnitTests.Managers
{
    public class WoWTokenManagerTests
    {
        private readonly Mock<IWoWTokenService> wowTokenService;
        private readonly WoWTokenManager manager;

        public WoWTokenManagerTests()
        {
            this.wowTokenService = new Mock<IWoWTokenService>();
            var loggerMock = new Mock<ILogger<WoWTokenManager>>();
            this.manager = new WoWTokenManager(this.wowTokenService.Object, loggerMock.Object);
        }

        [Fact]
        public void WoWTokenManager_GetAllWoWTokens_ReturnsTypeWoWTokenResponse()
        {
            // Arrange
            this.wowTokenService.Setup(m => m.GetAllWoWTokens()).ReturnsAsync(new WoWTokenResponse(new []{ new WoWTokenItem(), }));

            // Act
            var response = this.manager.GetAllWoWTokens();

            // Assert
            Assert.IsAssignableFrom<WoWTokenResponse>(response.Result);
        }

        [Fact]
        public void WoWTokenManager_GetAllWoWTokens_ReturnsWoWTokenTokenItem()
        {
            // Arrange
            const WowRegion region = WowRegion.Us;

            var expectedToken = new WoWTokenItem()
            {
                LastUpdatedTimestamp = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                Price = string.Empty,
                Region = region.ToString()
            };

            this.wowTokenService.Setup(m => m.GetAllWoWTokens()).ReturnsAsync(new WoWTokenResponse(new[] { expectedToken, }));

            // Act
            var response = this.manager.GetAllWoWTokens();

            // Assert
            var actualToken = response.Result.WowTokens.ToList().First();
            Assert.Equal(expectedToken.LastUpdatedTimestamp, actualToken.LastUpdatedTimestamp);
            Assert.Equal(expectedToken.Price, actualToken.Price);
            Assert.Equal(expectedToken.Region, actualToken.Region);
        }

        [Fact]
        public void WoWTokenManager_GetWoWTokenByRegion_ReturnsTypeWoWTokenItem()
        {
            // Arrange
            this.wowTokenService.Setup(m => m.GetWoWTokenByRegion(It.IsAny<WowRegion>())).ReturnsAsync(new WoWTokenItem());

            // Act
            var response = this.manager.GetWoWTokenByRegion(WowRegion.Us);

            // Assert
            Assert.IsAssignableFrom<WoWTokenItem>(response.Result);
        }

        [Fact]
        public void WoWTokenManager_GetWoWTokenByRegion_ReturnsWoWTokenTokenItem()
        {
            // Arrange
            const WowRegion region = WowRegion.Us;

            var expectedToken = new WoWTokenItem()
            {
                LastUpdatedTimestamp = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                Price = string.Empty,
                Region = region.ToString()
            };

            this.wowTokenService.Setup(m => m.GetWoWTokenByRegion(It.IsAny<WowRegion>())).ReturnsAsync(expectedToken);

            // Act
            var response = this.manager.GetWoWTokenByRegion(region);

            // Assert
            Assert.Equal(expectedToken.LastUpdatedTimestamp, response.Result.LastUpdatedTimestamp);
            Assert.Equal(expectedToken.Price, response.Result.Price);
            Assert.Equal(expectedToken.Region, response.Result.Region);
        }
    }
}
