using UnityEngine;

namespace Eagle
{
    public static class EagleSDK
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AfterSceneLoad()
        {
            GameObject sdk = new GameObject("EagleApplication");
            sdk.AddComponent<EagleApplication>();
        }

        public static bool IsEditor()
        {
#if UNITY_EDITOR
            return true;
#else
            return false;
#endif
        }
    }
}