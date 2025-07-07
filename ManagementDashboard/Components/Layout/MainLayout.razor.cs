using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ManagementDashboard.Components.Layout
{
    public partial class MainLayout
    {
        [Inject] public ManagementDashboard.Services.SettingsService SettingsService { get; set; } = default!;
        [Inject] public IJSRuntime JS { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await ApplyThemeAsync();
        }

        private async Task ApplyThemeAsync()
        {
            var theme = SettingsService.IsDarkMode ? "dark" : "light";
            await JS.InvokeVoidAsync("document.body.setAttribute", "data-bs-theme", theme);
        }
    }
}
