using System.Configuration;

namespace FactWeb.Infrastructure
{
    static class ConfigManager
    {
        public static string GetConfigurationSetting(string configName)
        {
            try
            {
                //if (RoleEnvironment.IsAvailable)
                //    return RoleEnvironment.GetConfigurationSettingValue(configName);
                //else
                    return ConfigurationManager.AppSettings[configName];
            }
            catch
            {
                return "";
            }
        }
    }
}
