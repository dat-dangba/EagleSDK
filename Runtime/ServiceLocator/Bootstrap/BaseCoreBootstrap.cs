using System.Threading.Tasks;
using UnityEngine;

namespace Eagle
{
    public abstract class BaseCoreBootstrap : BaseBootstrap
    {
        protected override ServiceContainer Container => ServiceLocator.Core;

        protected abstract void ManualInitializeServices();

        protected override async Task BootstrapAsync()
        {
            InitializeServices();
            ManualInitializeServices();
            await AsyncInitializeServices();
            IsReady = true;
#if UNITY_EDITOR
            Debug.LogWarning($"[{GetType().Name}] - Ready {IsReady}");
#endif
        }
    }
}
