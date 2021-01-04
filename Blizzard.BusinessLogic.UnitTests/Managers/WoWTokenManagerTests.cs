using System;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using MigsTech.Blizzard.BusinessLogic.Managers;
using MigsTech.Blizzard.Data.Models;
using MigsTech.Blizzard.Data.Services;
using Moq;
using Xunit;

namespace MigsTech.Blizzard.BusinessLogic.UnitTests.Managers
{
    public class WoWTokenManagerTests
    {
        private readonly Mock<IWoWTokenService> _wowTokenService;
        private readonly WoWTokenManager _manager;

        public WoWTokenManagerTests()
        {
            _wowTokenService = new Mock<IWoWTokenService>();
            _manager = new WoWTokenManager(NullLogger<WoWTokenManager>.Instance, _wowTokenService.Object);
        }

        [Fact]
        public void WoWTokenManager_GetAllWoWTokens_CallsGetAllWoWTokens()
        {
            // Arrange
            _wowTokenService.Setup(m => m.GetAllWoWTokens()).ReturnsAsync(new WoWTokenResponse(new []{ new WoWTokenItem(), }));

            // Act
            var response = _manager.GetAllWoWTokens();

            // Assert
            _wowTokenService.Verify(x => x.GetAllWoWTokens(), Times.Once);
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

            _wowTokenService.Setup(m => m.GetAllWoWTokens()).ReturnsAsync(new WoWTokenResponse(new[] { expectedToken, }));

            // Act
            var response = _manager.GetAllWoWTokens();

            // Assert
            var actualToken = response.Result.WowTokens.ToList().First();
            Assert.Equal(expectedToken.LastUpdatedTimestamp, actualToken.LastUpdatedTimestamp);
            Assert.Equal(expectedToken.Price, actualToken.Price);
            Assert.Equal(expectedToken.Region, actualToken.Region);
        }

        [Fact]
        public void WoWTokenManager_GetWoWTokenByRegion_CallsGetWoWTokenByRegion()
        {
            // Arrange
            _wowTokenService
                .Setup(m => m.GetWoWTokenByRegion(
                    It.Is<WowRegion>(x => x == WowRegion.Us)))
                .ReturnsAsync(new WoWTokenItem());

            // Act
            var response = _manager.GetWoWTokenByRegion(WowRegion.Us);

            // Assert
            _wowTokenService
                .Verify(x => x.GetWoWTokenByRegion(It.Is<WowRegion>(x => x == WowRegion.Us)), Times.Once);
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

            _wowTokenService.Setup(m => m.GetWoWTokenByRegion(It.IsAny<WowRegion>())).ReturnsAsync(expectedToken);

            // Act
            var response = _manager.GetWoWTokenByRegion(region);

            // Assert
            Assert.Equal(expectedToken.LastUpdatedTimestamp, response.Result.LastUpdatedTimestamp);
            Assert.Equal(expectedToken.Price, response.Result.Price);
            Assert.Equal(expectedToken.Region, response.Result.Region);
        }
    }
}
