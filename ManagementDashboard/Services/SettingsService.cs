using Microsoft.Maui.Storage;

namespace ManagementDashboard.Services
{
    public class SettingsService
    {
        private const string ThemeKey = "AppTheme";
        public event Action? OnThemeChanged;

        public bool IsDarkMode
        {
            get => Preferences.Get(ThemeKey, "light") == "dark";
            set
            {
                Preferences.Set(ThemeKey, value ? "dark" : "light");
                OnThemeChanged?.Invoke();
            }
        }

        public string GetTheme() => IsDarkMode ? "dark" : "light";
    }
}
