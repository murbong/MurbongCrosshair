using Windows.Storage;

namespace MurbongCrosshair
{
    public static class Settings
    {
        private static readonly ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;

        public static T Get<T>(string key, T defaultValue = default)
        {
            if (LocalSettings.Values.TryGetValue(key, out var value) && value is T typed)
            {
                return typed;
            }
            return defaultValue;
        }

        public static void Set<T>(string key, T value)
        {
            LocalSettings.Values[key] = value;
        }

        public static bool Contains(string key)
        {
            return LocalSettings.Values.ContainsKey(key);
        }

        public static void Remove(string key)
        {
            LocalSettings.Values.Remove(key);
        }

        public static void Clear()
        {
            LocalSettings.Values.Clear();
        }
    }
}
