using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MurbongCrosshair
{
    public static class Settings
    {
        private static ApplicationDataContainer getLocalSettings()
        {
            return ApplicationData.Current.LocalSettings;
        }
        public static object GetSettingValue(string key)
        {
            return getLocalSettings().Values[key];
        }
        public static void SetSettingValue(string key, string value)
        {
            getLocalSettings().Values[key] = value;
        }
    }
}
