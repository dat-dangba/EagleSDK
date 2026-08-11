using UnityEngine;

namespace Eagle
{
    public static class ServiceLocator
    {
        public static ServiceContainer Sdk { get; private set; }
        public static ServiceContainer Core { get; private set; }
        public static ServiceContainer Game { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            Core = new ServiceContainer();
            Sdk = new ServiceContainer();
            Game = new ServiceContainer();
        }
    }
}
