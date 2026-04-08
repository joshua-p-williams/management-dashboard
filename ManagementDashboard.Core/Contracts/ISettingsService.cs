using System;

namespace ManagementDashboard.Core.Contracts
{
    public interface ISettingsService
    {
        event Action? OnThemeChanged;
        bool IsDarkMode { get; set; }
        string GetTheme();
        int DueDateReminderThresholdDays { get; set; }

        // Azure Computer Vision Settings
        string? AzureVisionEndpoint { get; set; }
        Task<string?> GetAzureVisionApiKeyAsync();
        Task SetAzureVisionApiKeyAsync(string? apiKey);
        int MaxImageSizeMB { get; set; }
        Task<bool> IsAzureVisionConfiguredAsync(); // Make this async

        // Azure AI Speech Services Settings
        string? AzureSpeechRegion { get; set; }
        Task<string?> GetAzureSpeechSubscriptionKeyAsync();
        Task SetAzureSpeechSubscriptionKeyAsync(string? subscriptionKey);
        string AzureSpeechLanguage { get; set; }
        int MaxRecordingDurationMinutes { get; set; }
        Task<bool> IsAzureSpeechConfiguredAsync();
    }
}
