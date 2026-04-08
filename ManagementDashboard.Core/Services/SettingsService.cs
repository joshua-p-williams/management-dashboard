using System;
using ManagementDashboard.Core.Contracts;
using ManagementDashboard.Core.Constants;

namespace ManagementDashboard.Core.Services
{
    public class SettingsService : ISettingsService
    {
        private const string ThemeKey = "AppTheme";
        private const string DueDateReminderKey = "DueDateReminderThresholdDays";
        private const string AzureVisionEndpointKey = "AzureVisionEndpoint";
        private const string AzureVisionApiKeySecureKey = "AzureVisionApiKey";
        private const string MaxImageSizeMBKey = "MaxImageSizeMB";

        // Azure Speech Settings Keys
        private const string AzureSpeechRegionKey = "AzureSpeechRegion";
        private const string AzureSpeechSubscriptionKeySecureKey = "AzureSpeechSubscriptionKey";
        private const string AzureSpeechLanguageKey = "AzureSpeechLanguage";
        private const string MaxRecordingDurationMinutesKey = "MaxRecordingDurationMinutes";

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

        // Azure AI Speech Services Settings
        public string? AzureSpeechRegion
        {
            get => _preferences.Get(AzureSpeechRegionKey, AzureSpeechConstants.DefaultRegion);
            set => _preferences.Set(AzureSpeechRegionKey, value ?? AzureSpeechConstants.DefaultRegion);
        }

        public async Task<string?> GetAzureSpeechSubscriptionKeyAsync()
        {
            try
            {
                return await Microsoft.Maui.Storage.SecureStorage.GetAsync(AzureSpeechSubscriptionKeySecureKey);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task SetAzureSpeechSubscriptionKeyAsync(string? subscriptionKey)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(subscriptionKey))
                {
                    Microsoft.Maui.Storage.SecureStorage.Remove(AzureSpeechSubscriptionKeySecureKey);
                }
                else
                {
                    await Microsoft.Maui.Storage.SecureStorage.SetAsync(AzureSpeechSubscriptionKeySecureKey, subscriptionKey);
                }
            }
            catch (Exception)
            {
                // Handle SecureStorage failures gracefully
            }
        }

        public string AzureSpeechLanguage
        {
            get => _preferences.Get(AzureSpeechLanguageKey, AzureSpeechConstants.DefaultLanguage);
            set => _preferences.Set(AzureSpeechLanguageKey, value);
        }

        public int MaxRecordingDurationMinutes
        {
            get => _preferences.GetInt(MaxRecordingDurationMinutesKey, AzureSpeechConstants.DefaultMaxRecordingDurationMinutes);
            set => _preferences.SetInt(MaxRecordingDurationMinutesKey, value);
        }

        public async Task<bool> IsAzureSpeechConfiguredAsync()
        {
            var region = AzureSpeechRegion;
            var subscriptionKey = await GetAzureSpeechSubscriptionKeyAsync();

            return !string.IsNullOrWhiteSpace(region) && 
                   !string.IsNullOrWhiteSpace(subscriptionKey);
        }
    }
}
