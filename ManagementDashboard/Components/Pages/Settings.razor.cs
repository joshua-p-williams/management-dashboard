using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ManagementDashboard.Core.Services;
using ManagementDashboard.Core.Contracts;
using ManagementDashboard.Core.Constants;

namespace ManagementDashboard.Components.Pages
{
    public partial class Settings : ComponentBase
    {
        [Inject] public SettingsService SettingsService { get; set; } = default!;
        [Inject] public IJSRuntime JS { get; set; } = default!;
        [Inject] public IAzureVisionService AzureVisionService { get; set; } = default!;
        [Inject] public IAzureSpeechService AzureSpeechService { get; set; } = default!;

        protected bool IsDarkMode
        {
            get => SettingsService.IsDarkMode;
            set
            {
                if (SettingsService.IsDarkMode != value)
                {
                    SettingsService.IsDarkMode = value;
                    _ = ApplyThemeAsync();
                }
            }
        }

        protected int DueDateReminderThresholdDays
        {
            get => SettingsService.DueDateReminderThresholdDays;
            set
            {
                if (SettingsService.DueDateReminderThresholdDays != value)
                {
                    SettingsService.DueDateReminderThresholdDays = value;
                    StateHasChanged();
                }
            }
        }

        // Azure Computer Vision Settings - use local fields for editing
        private string _azureVisionEndpoint = string.Empty;
        private string _azureVisionApiKey = string.Empty;

        protected string AzureVisionEndpoint
        {
            get => _azureVisionEndpoint;
            set
            {
                if (_azureVisionEndpoint != value)
                {
                    _azureVisionEndpoint = value;
                    ValidateEndpoint();
                    HasUnsavedChanges = true;
                }
            }
        }

        protected string AzureVisionApiKey
        {
            get => _azureVisionApiKey;
            set
            {
                if (_azureVisionApiKey != value)
                {
                    _azureVisionApiKey = value;
                    ValidateApiKey();
                    HasUnsavedChanges = true;
                }
            }
        }

        protected int MaxImageSizeMB
        {
            get => SettingsService.MaxImageSizeMB;
            set
            {
                if (SettingsService.MaxImageSizeMB != value)
                {
                    SettingsService.MaxImageSizeMB = value;
                    StateHasChanged();
                }
            }
        }

        protected bool IsEndpointValid { get; set; } = true;
        protected bool IsApiKeyValid { get; set; } = true;
        protected bool IsAzureVisionConfigured { get; set; } = false;
        protected bool ShowConnectionResult { get; set; } = false;
        protected bool ConnectionTestSuccessful { get; set; } = false;
        protected string ConnectionTestMessage { get; set; } = string.Empty;
        protected bool HasUnsavedChanges { get; set; } = false;

        // Azure Speech Services Settings - use local fields for editing
        private string _azureSpeechSubscriptionKey = string.Empty;
        private string _azureSpeechRegion = string.Empty;

        protected string AzureSpeechSubscriptionKey
        {
            get => _azureSpeechSubscriptionKey;
            set
            {
                if (_azureSpeechSubscriptionKey != value)
                {
                    _azureSpeechSubscriptionKey = value;
                    ValidateSpeechKey();
                    HasUnsavedSpeechChanges = true;
                }
            }
        }

        protected string AzureSpeechRegion
        {
            get => _azureSpeechRegion;
            set
            {
                if (_azureSpeechRegion != value)
                {
                    _azureSpeechRegion = value;
                    ValidateSpeechRegion();
                    HasUnsavedSpeechChanges = true;
                }
            }
        }

        protected string AzureSpeechLanguage
        {
            get => SettingsService.AzureSpeechLanguage;
            set
            {
                if (SettingsService.AzureSpeechLanguage != value)
                {
                    SettingsService.AzureSpeechLanguage = value;
                    StateHasChanged();
                }
            }
        }

        protected int MaxRecordingDurationMinutes
        {
            get => SettingsService.MaxRecordingDurationMinutes;
            set
            {
                if (SettingsService.MaxRecordingDurationMinutes != value)
                {
                    SettingsService.MaxRecordingDurationMinutes = value;
                    StateHasChanged();
                }
            }
        }

        protected bool IsSpeechKeyValid { get; set; } = true;
        protected bool IsSpeechRegionValid { get; set; } = true;
        protected bool IsAzureSpeechConfigured { get; set; } = false;
        protected bool ShowSpeechConnectionResult { get; set; } = false;
        protected bool SpeechConnectionTestSuccessful { get; set; } = false;
        protected string SpeechConnectionTestMessage { get; set; } = string.Empty;
        protected bool HasUnsavedSpeechChanges { get; set; } = false;

        // Available regions and languages for dropdowns
        protected Dictionary<string, string> AvailableRegions => AzureSpeechConstants.Regions.RegionDisplayNames;
        protected Dictionary<string, string> AvailableLanguages => AzureSpeechConstants.Languages.LanguageDisplayNames;

        protected override async Task OnInitializedAsync()
        {
            await ApplyThemeAsync();

            // Load current values into local fields
            _azureVisionEndpoint = SettingsService.AzureVisionEndpoint ?? string.Empty;
            _azureVisionApiKey = await SettingsService.GetAzureVisionApiKeyAsync() ?? string.Empty;

            // Load Azure Speech Services values
            _azureSpeechSubscriptionKey = await SettingsService.GetAzureSpeechSubscriptionKeyAsync() ?? string.Empty;
            _azureSpeechRegion = SettingsService.AzureSpeechRegion ?? string.Empty;

            // Check if Azure Vision is configured
            IsAzureVisionConfigured = await SettingsService.IsAzureVisionConfiguredAsync();

            // Check if Azure Speech is configured
            IsAzureSpeechConfigured = await SettingsService.IsAzureSpeechConfiguredAsync();

            // Validate after loading values
            ValidateEndpoint();
            ValidateApiKey();
            ValidateSpeechKey();
            ValidateSpeechRegion();
            StateHasChanged();
        }

        private async Task ApplyThemeAsync()
        {
            var theme = SettingsService.IsDarkMode ? "dark" : "light";
            await JS.InvokeVoidAsync("document.body.setAttribute", "data-bs-theme", theme);
        }

        protected void ValidateEndpoint()
        {
            var endpoint = AzureVisionEndpoint?.Trim();

            // Much more lenient validation - just check for HTTPS URL
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                IsEndpointValid = false;
            }
            else if (!endpoint.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                IsEndpointValid = false;
            }
            else if (!Uri.TryCreate(endpoint, UriKind.Absolute, out Uri? uri) || uri.Scheme != "https")
            {
                IsEndpointValid = false;
            }
            else
            {
                IsEndpointValid = true;
            }

            StateHasChanged();
        }

        protected void ValidateApiKey()
        {
            IsApiKeyValid = !string.IsNullOrWhiteSpace(AzureVisionApiKey?.Trim());
            StateHasChanged();
        }

        protected async Task SaveAzureSettings()
        {
            try
            {
                // Save settings
                SettingsService.AzureVisionEndpoint = AzureVisionEndpoint?.Trim();
                await SettingsService.SetAzureVisionApiKeyAsync(AzureVisionApiKey?.Trim());

                // Update configuration status
                IsAzureVisionConfigured = await SettingsService.IsAzureVisionConfiguredAsync();
                HasUnsavedChanges = false;

                // Show success message
                ShowConnectionResult = true;
                ConnectionTestSuccessful = true;
                ConnectionTestMessage = "Settings saved successfully!";

                StateHasChanged();

                // Hide success message after 3 seconds
                _ = Task.Delay(3000).ContinueWith(_ =>
                {
                    ShowConnectionResult = false;
                    InvokeAsync(StateHasChanged);
                });
            }
            catch (Exception ex)
            {
                ShowConnectionResult = true;
                ConnectionTestSuccessful = false;
                ConnectionTestMessage = $"Failed to save settings: {ex.Message}";
                StateHasChanged();
            }
        }

        protected async Task TestAzureConnection()
        {
            // Save first, then test
            await SaveAzureSettings();

            if (!IsAzureVisionConfigured)
            {
                ShowConnectionResult = true;
                ConnectionTestSuccessful = false;
                ConnectionTestMessage = "Please save valid credentials before testing connection.";
                StateHasChanged();
                return;
            }

            ShowConnectionResult = true;
            ConnectionTestSuccessful = false;
            ConnectionTestMessage = "Testing connection...";
            StateHasChanged();

            try
            {
                var isConfigured = await AzureVisionService.IsConfiguredAsync();
                if (isConfigured)
                {
                    ConnectionTestSuccessful = true;
                    ConnectionTestMessage = "Connection test successful! Azure Computer Vision is properly configured.";
                }
                else
                {
                    ConnectionTestSuccessful = false;
                    ConnectionTestMessage = "Configuration incomplete. Please check your endpoint URL and API key.";
                }
            }
            catch (Exception ex)
            {
                ConnectionTestSuccessful = false;
                ConnectionTestMessage = $"Connection test failed: {ex.Message}";
            }

            StateHasChanged();
        }

        protected void ValidateSpeechKey()
        {
            IsSpeechKeyValid = !string.IsNullOrWhiteSpace(AzureSpeechSubscriptionKey?.Trim());
            StateHasChanged();
        }

        protected void ValidateSpeechRegion()
        {
            IsSpeechRegionValid = !string.IsNullOrWhiteSpace(AzureSpeechRegion?.Trim()) && 
                                  AvailableRegions.ContainsKey(AzureSpeechRegion.Trim());
            StateHasChanged();
        }

        protected async Task SaveSpeechSettings()
        {
            try
            {
                // Save speech settings
                SettingsService.AzureSpeechRegion = AzureSpeechRegion?.Trim();
                await SettingsService.SetAzureSpeechSubscriptionKeyAsync(AzureSpeechSubscriptionKey?.Trim());

                // Update configuration status
                IsAzureSpeechConfigured = await SettingsService.IsAzureSpeechConfiguredAsync();
                HasUnsavedSpeechChanges = false;

                // Show success message
                ShowSpeechConnectionResult = true;
                SpeechConnectionTestSuccessful = true;
                SpeechConnectionTestMessage = "Settings saved successfully!";

                StateHasChanged();

                // Hide success message after 3 seconds
                _ = Task.Delay(3000).ContinueWith(_ =>
                {
                    ShowSpeechConnectionResult = false;
                    InvokeAsync(StateHasChanged);
                });
            }
            catch (Exception ex)
            {
                ShowSpeechConnectionResult = true;
                SpeechConnectionTestSuccessful = false;
                SpeechConnectionTestMessage = $"Failed to save settings: {ex.Message}";
                StateHasChanged();
            }
        }

        protected async Task TestSpeechConnection()
        {
            // Save first, then test
            await SaveSpeechSettings();

            if (!IsAzureSpeechConfigured)
            {
                ShowSpeechConnectionResult = true;
                SpeechConnectionTestSuccessful = false;
                SpeechConnectionTestMessage = "Please save valid credentials before testing connection.";
                StateHasChanged();
                return;
            }

            ShowSpeechConnectionResult = true;
            SpeechConnectionTestSuccessful = false;
            SpeechConnectionTestMessage = "Testing connection...";
            StateHasChanged();

            try
            {
                var isConfigured = await AzureSpeechService.IsConfiguredAsync();
                if (isConfigured)
                {
                    SpeechConnectionTestSuccessful = true;
                    SpeechConnectionTestMessage = "Connection test successful! Azure Speech Services is properly configured.";
                }
                else
                {
                    SpeechConnectionTestSuccessful = false;
                    SpeechConnectionTestMessage = "Configuration incomplete. Please check your subscription key and region.";
                }
            }
            catch (Exception ex)
            {
                SpeechConnectionTestSuccessful = false;
                SpeechConnectionTestMessage = $"Connection test failed: {ex.Message}";
            }

            StateHasChanged();
        }
    }
}
