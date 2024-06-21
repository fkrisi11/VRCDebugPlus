using System.Configuration;
using System.Drawing;

namespace VRCDebug
{
    public static class ConfigManager
    {
        public static bool LoadIsNightMode()
        {
            EnsureConfigValueExists("IsNightMode", "true");
            bool.TryParse(ConfigurationManager.AppSettings["IsNightMode"], out bool isNightMode);
            return isNightMode;
        }

        public static void SaveIsNightMode(bool isNightMode)
        {
            UpdateAppSettings("IsNightMode", isNightMode.ToString());
        }

        public static Size LoadWindowSize()
        {
            EnsureConfigValueExists("WindowWidth", "1216");
            EnsureConfigValueExists("WindowHeight", "606");

            int.TryParse(ConfigurationManager.AppSettings["WindowWidth"], out int width);
            int.TryParse(ConfigurationManager.AppSettings["WindowHeight"], out int height);

            return new Size(width, height);
        }

        public static void SaveWindowSize(int width, int height)
        {
            UpdateAppSettings("WindowWidth", width.ToString());
            UpdateAppSettings("WindowHeight", height.ToString());
        }

        // If a checked value doesn't exist already, create it
        private static void EnsureConfigValueExists(string key, string defaultValue)
        {
            if (ConfigurationManager.AppSettings[key] == null)
                UpdateAppSettings(key, defaultValue);
        }

        private static void UpdateAppSettings(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (config.AppSettings.Settings[key] == null)
                config.AppSettings.Settings.Add(key, value);
            else
                config.AppSettings.Settings[key].Value = value;

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }

}
