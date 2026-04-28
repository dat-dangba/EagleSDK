using System;
using UnityEngine;

namespace Eagle
{
    public class EagleApplication : MonoBehaviour
    {
        public static event Action OnAppUpdate;
        public static event Action OnAppStart;
        public static event Action OnAppEnable;
        public static event Action OnAppDisable;
        public static event Action<bool> OnAppPause;
        public static event Action<bool> OnAppFocus;

        private void OnApplicationPause(bool pause)
        {
            OnAppPause?.Invoke(pause);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            OnAppFocus?.Invoke(hasFocus);
        }

        private void Start()
        {
            OnAppStart?.Invoke();
        }

        private void Update()
        {
            OnAppUpdate?.Invoke();
        }

        private void OnEnable()
        {
            OnAppEnable?.Invoke();
        }

        private void OnDisable()
        {
            OnAppDisable?.Invoke();
        }
    }
}