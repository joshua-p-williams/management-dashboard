using System;
using System.Globalization;

namespace ManagementDashboard.Data.Services
{
    public static class DataTransformationService
    {
        // Converts an object (DateTime or string) to a SQLite date string (yyyy-MM-dd)
        public static string ToSqliteDateString(object value)
        {
            if (value == null) value = DateTime.Now;
            if (value is DateTime dt)
                return dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (value is string str)
            {
                if (DateTime.TryParse(str, out var parsed))
                    return parsed.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                return str; // fallback: return as-is
            }
            return value.ToString() ?? string.Empty;
        }

        // Converts an object to a boolean, handling various string representations
        public static bool ToBool(object? value)
        {
            if (value == null) return false;
            if (value is bool b) return b;
            if (value is int i) return i != 0;
            if (value is string str)
            {
                str = str.Trim().ToLowerInvariant();
                if (str == "true" || str == "t" || str == "1" || str == "yes" || str == "y") return true;
                if (str == "false" || str == "f" || str == "0" || str == "no" || str == "n") return false;
            }
            return false;
        }
    }
}
