using System;
using System.Collections.Generic;
using Xunit;
using ManagementDashboard.Core.Services;
using ManagementDashboard.Core.Contracts;
using ManagementDashboard.Core.Constants;

namespace ManagementDashboard.Tests
{
    public class InMemoryPreferences : IAppPreferences
    {
        private readonly Dictionary<string, string> _store = new();
        private readonly Dictionary<string, int> _intStore = new();
        public string Get(string key, string defaultValue) => _store.TryGetValue(key, out var v) ? v : defaultValue;
        public void Set(string key, string value) => _store[key] = value;
        public int GetInt(string key, int defaultValue) => _intStore.TryGetValue(key, out var v) ? v : defaultValue;
        public void SetInt(string key, int value) => _intStore[key] = value;
    }

    public class SettingsServiceTests
    {
        [Fact]
        public void Theme_DefaultsToLight()
        {
            var service = new SettingsService(new InMemoryPreferences());
            Assert.False(service.IsDarkMode);
            Assert.Equal("light", service.GetTheme());
        }

        [Fact]
        public void CanSetDarkMode()
        {
            var service = new SettingsService(new InMemoryPreferences());
            service.IsDarkMode = true;
            Assert.True(service.IsDarkMode);
            Assert.Equal("dark", service.GetTheme());
        }

        [Fact]
        public void OnThemeChanged_IsInvoked()
        {
            var service = new SettingsService(new InMemoryPreferences());
            bool invoked = false;
            service.OnThemeChanged += () => invoked = true;
            service.IsDarkMode = true;
            Assert.True(invoked);
        }

        [Fact]
        public void DueDateReminderThreshold_DefaultsTo3()
        {
            var prefs = new InMemoryPreferences();
            var service = new SettingsService(prefs);
            Assert.Equal(3, service.DueDateReminderThresholdDays);
        }

        [Fact]
        public void CanSetDueDateReminderThreshold()
        {
            var prefs = new InMemoryPreferences();
            var service = new SettingsService(prefs);
            service.DueDateReminderThresholdDays = 5;
            Assert.Equal(5, service.DueDateReminderThresholdDays);
            // Also check persistence
            var service2 = new SettingsService(prefs);
            Assert.Equal(5, service2.DueDateReminderThresholdDays);
        }

        // Azure Speech Settings Tests
        [Fact]
        public void AzureSpeechRegion_DefaultsToEastUs()
        {
            var service = new SettingsService(new InMemoryPreferences());
            Assert.Equal(AzureSpeechConstants.DefaultRegion, service.AzureSpeechRegion);
        }

        [Fact]
        public void CanSetAzureSpeechRegion()
        {
            var prefs = new InMemoryPreferences();
            var service = new SettingsService(prefs);
            service.AzureSpeechRegion = AzureSpeechConstants.Regions.WestUs2;
            Assert.Equal(AzureSpeechConstants.Regions.WestUs2, service.AzureSpeechRegion);

            // Check persistence
            var service2 = new SettingsService(prefs);
            Assert.Equal(AzureSpeechConstants.Regions.WestUs2, service2.AzureSpeechRegion);
        }

        [Fact]
        public void AzureSpeechLanguage_DefaultsToEnglishUs()
        {
            var service = new SettingsService(new InMemoryPreferences());
            Assert.Equal(AzureSpeechConstants.DefaultLanguage, service.AzureSpeechLanguage);
        }

        [Fact]
        public void CanSetAzureSpeechLanguage()
        {
            var prefs = new InMemoryPreferences();
            var service = new SettingsService(prefs);
            service.AzureSpeechLanguage = AzureSpeechConstants.Languages.SpanishSpain;
            Assert.Equal(AzureSpeechConstants.Languages.SpanishSpain, service.AzureSpeechLanguage);

            // Check persistence
            var service2 = new SettingsService(prefs);
            Assert.Equal(AzureSpeechConstants.Languages.SpanishSpain, service2.AzureSpeechLanguage);
        }

        [Fact]
        public void MaxRecordingDurationMinutes_DefaultsToFive()
        {
            var service = new SettingsService(new InMemoryPreferences());
            Assert.Equal(AzureSpeechConstants.DefaultMaxRecordingDurationMinutes, service.MaxRecordingDurationMinutes);
        }

        [Fact]
        public void CanSetMaxRecordingDurationMinutes()
        {
            var prefs = new InMemoryPreferences();
            var service = new SettingsService(prefs);
            service.MaxRecordingDurationMinutes = 10;
            Assert.Equal(10, service.MaxRecordingDurationMinutes);

            // Check persistence
            var service2 = new SettingsService(prefs);
            Assert.Equal(10, service2.MaxRecordingDurationMinutes);
        }

        [Fact]
        public async void IsAzureSpeechConfiguredAsync_ReturnsFalseWithoutCredentials()
        {
            var service = new SettingsService(new InMemoryPreferences());
            var isConfigured = await service.IsAzureSpeechConfiguredAsync();
            Assert.False(isConfigured);
        }

        // Note: We can't easily test the secure storage methods in unit tests 
        // since they depend on platform-specific implementations
        // These would be tested in integration tests or manual testing
    }
}
