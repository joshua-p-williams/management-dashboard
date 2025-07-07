using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ManagementDashboard.Services;

namespace ManagementDashboard.Components.Pages
{
    public partial class Settings : ComponentBase
    {
        [Inject] public SettingsService SettingsService { get; set; } = default!;
        [Inject] public IJSRuntime JS { get; set; } = default!;

        protected bool IsDarkMode
        {
            get => SettingsService.IsDarkMode;
            set
            {
                if (SettingsService.IsDarkMode != value)
                {
                    SettingsService.IsDarkMode = value;
                    ApplyThemeAsync();
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await ApplyThemeAsync();
        }

        private async Task ApplyThemeAsync()
        {
            var theme = SettingsService.IsDarkMode ? "dark" : "light";
            await JS.InvokeVoidAsync("document.body.setAttribute", "data-bs-theme", theme);
            StateHasChanged();
        }
    }
}
