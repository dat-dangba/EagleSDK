namespace Eagle
{
    using UnityEngine;

    public enum LogLevel
    {
        None,
        Minimal,
        Verbose
    }

    internal static class EagleLog
    {
        public static LogLevel CurrentLogLevel
        {
            get
            {
                var logConfig = EagleConfigService.GetConfig<GeneralConfig>();
                return logConfig != null ? logConfig.LogLevel : LogLevel.Minimal;
            }
        }

        public static void Log(string message, LogLevel level = LogLevel.Minimal)
        {
            if (ShouldLog(level))
            {
                Debug.Log($"EagleSDK - {message}");
            }
        }

        public static void LogWarning(string message, LogLevel level = LogLevel.Minimal)
        {
            if (ShouldLog(level))
            {
                Debug.LogWarning($"EagleSDK - {message}");
            }
        }

        public static void LogError(string message, LogLevel level = LogLevel.Minimal)
        {
            if (ShouldLog(level))
            {
                Debug.LogError($"EagleSDK - {message}");
            }
        }

        private static bool ShouldLog(LogLevel messageLevel)
        {
            if (CurrentLogLevel == LogLevel.None)
            {
                return false;
            }

            if (CurrentLogLevel == LogLevel.Verbose)
            {
                return true;
            }

            if (CurrentLogLevel == LogLevel.Minimal && messageLevel == LogLevel.Minimal)
            {
                return true;
            }

            return false;
        }
    }
}