using System;

namespace ManagementDashboard.Core.Contracts
{
    public interface ISettingsService
    {
        event Action? OnThemeChanged;
        bool IsDarkMode { get; set; }
        string GetTheme();
        int OverdueThresholdDays { get; set; }
    }
}
