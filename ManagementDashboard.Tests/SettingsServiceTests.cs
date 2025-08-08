using System;
using System.Collections.Generic;
using Xunit;
using ManagementDashboard.Core.Services;
using ManagementDashboard.Core.Contracts;

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
    }
}
