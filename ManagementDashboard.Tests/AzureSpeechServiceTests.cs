using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using ManagementDashboard.Core.Services;
using ManagementDashboard.Core.Contracts;
using ManagementDashboard.Core.Constants;

namespace ManagementDashboard.Tests
{
    public class AzureSpeechServiceTests
    {
        [Fact]
        public async Task IsConfiguredAsync_WithValidCredentials_ReturnsTrue()
        {
            // Arrange
            var mockSettings = new Mock<ISettingsService>();
            mockSettings.Setup(s => s.AzureSpeechRegion).Returns("eastus");
            mockSettings.Setup(s => s.GetAzureSpeechSubscriptionKeyAsync()).ReturnsAsync("test-subscription-key");

            var azureSpeechService = new AzureSpeechService(mockSettings.Object);

            // Act
            var result = await azureSpeechService.IsConfiguredAsync();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsConfiguredAsync_WithMissingRegion_ReturnsFalse()
        {
            // Arrange
            var mockSettings = new Mock<ISettingsService>();
            mockSettings.Setup(s => s.AzureSpeechRegion).Returns((string?)null);
            mockSettings.Setup(s => s.GetAzureSpeechSubscriptionKeyAsync()).ReturnsAsync("test-subscription-key");

            var azureSpeechService = new AzureSpeechService(mockSettings.Object);

            // Act
            var result = await azureSpeechService.IsConfiguredAsync();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task IsConfiguredAsync_WithMissingSubscriptionKey_ReturnsFalse()
        {
            // Arrange
            var mockSettings = new Mock<ISettingsService>();
            mockSettings.Setup(s => s.AzureSpeechRegion).Returns("eastus");
            mockSettings.Setup(s => s.GetAzureSpeechSubscriptionKeyAsync()).ReturnsAsync((string?)null);

            var azureSpeechService = new AzureSpeechService(mockSettings.Object);

            // Act
            var result = await azureSpeechService.IsConfiguredAsync();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetRecognitionLanguage_WithConfiguredLanguage_ReturnsLanguage()
        {
            // Arrange
            var mockSettings = new Mock<ISettingsService>();
            mockSettings.Setup(s => s.AzureSpeechLanguage).Returns("es-ES");

            var azureSpeechService = new AzureSpeechService(mockSettings.Object);

            // Act
            var result = azureSpeechService.GetRecognitionLanguage();

            // Assert
            Assert.Equal("es-ES", result);
        }

        [Fact]
        public void GetRecognitionLanguage_WithoutConfiguredLanguage_ReturnsDefault()
        {
            // Arrange
            var mockSettings = new Mock<ISettingsService>();
            mockSettings.Setup(s => s.AzureSpeechLanguage).Returns((string?)null);

            var azureSpeechService = new AzureSpeechService(mockSettings.Object);

            // Act
            var result = azureSpeechService.GetRecognitionLanguage();

            // Assert
            Assert.Equal(AzureSpeechConstants.DefaultLanguage, result);
        }

        [Fact]
        public async Task TranscribeSpeechAsync_WithoutConfiguration_ReturnsEmptyString()
        {
            // Arrange
            var mockSettings = new Mock<ISettingsService>();
            mockSettings.Setup(s => s.AzureSpeechRegion).Returns((string?)null);
            mockSettings.Setup(s => s.GetAzureSpeechSubscriptionKeyAsync()).ReturnsAsync((string?)null);

            var azureSpeechService = new AzureSpeechService(mockSettings.Object);
            var dummyAudioData = new byte[] { 0x01, 0x02, 0x03 };

            // Act
            var result = await azureSpeechService.TranscribeSpeechAsync(dummyAudioData);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        // Note: We can't easily test the actual speech recognition methods in unit tests 
        // since they depend on Azure services and audio processing
        // These would be tested in integration tests with real Azure credentials
    }
}