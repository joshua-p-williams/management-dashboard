using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ManagementDashboard.Core.Services;
using ManagementDashboard.Core.Contracts;

namespace ManagementDashboard.Components.Pages
{
    public partial class Settings : ComponentBase
    {
        [Inject] public SettingsService SettingsService { get; set; } = default!;
        [Inject] public IJSRuntime JS { get; set; } = default!;
        [Inject] public IAzureVisionService AzureVisionService { get; set; } = default!;

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

        protected override async Task OnInitializedAsync()
        {
            await ApplyThemeAsync();

            // Load current values into local fields
            _azureVisionEndpoint = SettingsService.AzureVisionEndpoint ?? string.Empty;
            _azureVisionApiKey = await SettingsService.GetAzureVisionApiKeyAsync() ?? string.Empty;

            // Check if Azure Vision is configured
            IsAzureVisionConfigured = await SettingsService.IsAzureVisionConfiguredAsync();

            // Validate after loading values
            ValidateEndpoint();
            ValidateApiKey();
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
    }
}
