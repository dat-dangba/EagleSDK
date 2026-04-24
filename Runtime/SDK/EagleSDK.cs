using System;
using System.Collections.Generic;
#if UNITY_ANDROID && UNITY_2019_2_OR_NEWER
// using AdjustSdk;
#endif
using UnityEngine;

namespace Eagle
{
    public enum Await
    {
        Adjust,
        Max,
        RemoteConfig
    }

    public static class EagleSDK
    {
        public static bool IsInitialized;

        public static event Action OnInitialized;

        // public static string Version = "1.0.0";

        private static List<Await> awaits = new()
        {
            Await.Adjust
        };

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
#if UNITY_ANDROID && UNITY_2019_2_OR_NEWER
            // Application.deepLinkActivated += deeplink => { Adjust.ProcessDeeplink(new AdjustDeeplink(deeplink)); };
#endif
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AfterSceneLoad()
        {
            GameObject sdk = new GameObject("EagleApplication");
            sdk.AddComponent<EagleApplication>();

#if UNITY_ANDROID && UNITY_2019_2_OR_NEWER
            // if (!string.IsNullOrEmpty(Application.absoluteURL))
            // {
            //     Adjust.ProcessDeeplink(new AdjustDeeplink(Application.absoluteURL));
            // }
#endif
            Initialize();
        }

        private static void Initialize()
        {
            // EagleAnalytics.Initialize(adId =>
            // {
            //     EagleLog.Log($"adId: {adId}");
            //     OnInitModuleComplete(Await.Adjust);
            // });
        }

        private static void OnInitModuleComplete(Await await)
        {
            if (awaits.Contains(await))
            {
                awaits.Remove(await);
            }

            if (awaits.Count > 0) return;
            IsInitialized = true;
            OnInitialized?.Invoke();
        }

        public static bool IsEditor()
        {
#if UNITY_EDITOR
            EagleLog.Log("[EagleAnalytics]: SDK can not be used in Editor.");
            return true;
#else
            return false;
#endif
        }
    }
}