namespace ManagementDashboard.Core.Contracts
{
    public interface IAppPreferences
    {
        string Get(string key, string defaultValue);
        void Set(string key, string value);
        int GetInt(string key, int defaultValue);
        void SetInt(string key, int value);
    }
}
