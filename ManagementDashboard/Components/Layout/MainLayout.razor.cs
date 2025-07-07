using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using ManagementDashboard.Core.Services;

namespace ManagementDashboard.Components.Layout
{
    public partial class MainLayout : IDisposable
    {
        [Inject] public SettingsService SettingsService { get; set; } = default!;
        [Inject] public IJSRuntime JS { get; set; } = default!;
        [Inject] public NavigationManager Navigation { get; set; } = default!;

        private bool isSidebarOpen = false;

        protected override void OnInitialized()
        {
            Navigation.LocationChanged += HandleLocationChanged;
        }

        private void HandleLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            isSidebarOpen = false;
            InvokeAsync(StateHasChanged);
        }

        private void ToggleSidebar() => isSidebarOpen = !isSidebarOpen;
        private void CloseSidebar() => isSidebarOpen = false;

        protected override async Task OnInitializedAsync()
        {
            await ApplyThemeAsync();
        }

        private async Task ApplyThemeAsync()
        {
            var theme = SettingsService.IsDarkMode ? "dark" : "light";
            await JS.InvokeVoidAsync("document.body.setAttribute", "data-bs-theme", theme);
        }

        public void Dispose()
        {
            Navigation.LocationChanged -= HandleLocationChanged;
        }
    }
}
