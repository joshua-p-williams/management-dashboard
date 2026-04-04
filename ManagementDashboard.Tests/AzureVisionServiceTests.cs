using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using ManagementDashboard.Core.Services;
using ManagementDashboard.Core.Contracts;

namespace ManagementDashboard.Tests
{
    public class AzureVisionServiceTests
    {
        [Fact]
        public async Task IsConfiguredAsync_WithValidCredentials_ReturnsTrue()
        {
            // Arrange
            var mockSettings = new Mock<ISettingsService>();
            mockSettings.Setup(s => s.AzureVisionEndpoint).Returns("https://test.api.cognitive.microsoft.com/");
            mockSettings.Setup(s => s.GetAzureVisionApiKeyAsync()).ReturnsAsync("test-api-key");

            var azureVisionService = new AzureVisionService(mockSettings.Object);

            // Act
            var result = await azureVisionService.IsConfiguredAsync();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsConfiguredAsync_WithMissingEndpoint_ReturnsFalse()
        {
            // Arrange
            var mockSettings = new Mock<ISettingsService>();
            mockSettings.Setup(s => s.AzureVisionEndpoint).Returns((string?)null);
            mockSettings.Setup(s => s.GetAzureVisionApiKeyAsync()).ReturnsAsync("test-api-key");

            var azureVisionService = new AzureVisionService(mockSettings.Object);

            // Act
            var result = await azureVisionService.IsConfiguredAsync();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task IsConfiguredAsync_WithMissingApiKey_ReturnsFalse()
        {
            // Arrange
            var mockSettings = new Mock<ISettingsService>();
            mockSettings.Setup(s => s.AzureVisionEndpoint).Returns("https://test.api.cognitive.microsoft.com/");
            mockSettings.Setup(s => s.GetAzureVisionApiKeyAsync()).ReturnsAsync((string?)null);

            var azureVisionService = new AzureVisionService(mockSettings.Object);

            // Act
            var result = await azureVisionService.IsConfiguredAsync();

            // Assert
            Assert.False(result);
        }
    }
}