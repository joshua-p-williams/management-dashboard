using System;
using ManagementDashboard.Core.Contracts;

namespace ManagementDashboard.Core.Services
{
    public class SettingsService : ISettingsService
    {
        private const string ThemeKey = "AppTheme";
        private const string DueDateReminderKey = "DueDateReminderThresholdDays";
        private const string AzureVisionEndpointKey = "AzureVisionEndpoint";
        private const string AzureVisionApiKeySecureKey = "AzureVisionApiKey";
        private const string MaxImageSizeMBKey = "MaxImageSizeMB";

        private readonly IAppPreferences _preferences;
        public event Action? OnThemeChanged;

        public SettingsService(IAppPreferences preferences)
        {
            _preferences = preferences;
        }

        public bool IsDarkMode
        {
            get => _preferences.Get(ThemeKey, "light") == "dark";
            set
            {
                _preferences.Set(ThemeKey, value ? "dark" : "light");
                OnThemeChanged?.Invoke();
            }
        }

        public string GetTheme() => IsDarkMode ? "dark" : "light";

        public int DueDateReminderThresholdDays
        {
            get => _preferences.GetInt(DueDateReminderKey, 3);
            set => _preferences.SetInt(DueDateReminderKey, value);
        }

        // Azure Computer Vision Settings
        public string? AzureVisionEndpoint
        {
            get => _preferences.Get(AzureVisionEndpointKey, null);
            set => _preferences.Set(AzureVisionEndpointKey, value ?? string.Empty);
        }

        public async Task<string?> GetAzureVisionApiKeyAsync()
        {
            try
            {
                return await Microsoft.Maui.Storage.SecureStorage.GetAsync(AzureVisionApiKeySecureKey);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task SetAzureVisionApiKeyAsync(string? apiKey)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    Microsoft.Maui.Storage.SecureStorage.Remove(AzureVisionApiKeySecureKey);
                }
                else
                {
                    await Microsoft.Maui.Storage.SecureStorage.SetAsync(AzureVisionApiKeySecureKey, apiKey);
                }
            }
            catch (Exception)
            {
                // Handle SecureStorage failures gracefully
            }
        }

        public int MaxImageSizeMB
        {
            get => _preferences.GetInt(MaxImageSizeMBKey, 5);
            set => _preferences.SetInt(MaxImageSizeMBKey, value);
        }

        public async Task<bool> IsAzureVisionConfiguredAsync()
        {
            var endpoint = AzureVisionEndpoint;
            var apiKey = await GetAzureVisionApiKeyAsync();

            return !string.IsNullOrWhiteSpace(endpoint) && 
                   !string.IsNullOrWhiteSpace(apiKey);
        }
    }
}
