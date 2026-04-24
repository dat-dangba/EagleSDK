namespace Eagle
{
    public static partial class GameConfig
    {
        public static T GetConfig<T>(string key, T defaultValue)
        {
#if UNITY_EDITOR
            return defaultValue;
#elif UNITY_ANDROID
            return GetConfigAndroid(key, defaultValue);
#elif UNITY_IOS
            return GetConfigIOS(key, defaultValue);
#else
            return defaultValue;
#endif
        }
    }
}