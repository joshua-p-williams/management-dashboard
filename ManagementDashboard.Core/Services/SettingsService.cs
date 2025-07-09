using System;
using ManagementDashboard.Core.Contracts;

namespace ManagementDashboard.Core.Services
{
    public class SettingsService : ISettingsService
    {
        private const string ThemeKey = "AppTheme";
        private const string OverdueKey = "OverdueThresholdDays";
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

        public int OverdueThresholdDays
        {
            get => _preferences.GetInt(OverdueKey, 2);
            set => _preferences.SetInt(OverdueKey, value);
        }
    }
}
