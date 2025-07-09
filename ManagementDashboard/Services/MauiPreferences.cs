using ManagementDashboard.Core.Contracts;

namespace ManagementDashboard.Services
{
    public class MauiPreferences : IAppPreferences
    {
        public string Get(string key, string defaultValue) => Preferences.Get(key, defaultValue);
        public void Set(string key, string value) => Preferences.Set(key, value);
        public int GetInt(string key, int defaultValue) => Preferences.Get(key, defaultValue);
        public void SetInt(string key, int value) => Preferences.Set(key, value);
    }
}
