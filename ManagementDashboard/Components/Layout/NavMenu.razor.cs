using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace ManagementDashboard.Components.Layout
{
    public partial class NavMenu : ComponentBase
    {
        protected bool IsDarkMode { get; set; } = false;

        [Inject] protected IJSRuntime? JS { get; set; }

        protected void ToggleTheme()
        {
            IsDarkMode = !IsDarkMode;
            var theme = IsDarkMode ? "dark" : "light";
            JS?.InvokeVoidAsync("document.body.setAttribute", "data-bs-theme", theme);
        }
    }
}
