using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Eagle
{
    public abstract class BaseBootstrap : MonoBehaviour
    {
        [SerializeField] protected List<MonoBehaviour> _Services;

        public bool IsReady { get; protected set; }

        protected abstract ServiceContainer Container { get; }

#if UNITY_EDITOR
        private void Reset()
        {
            LoadAllService();
        }

        protected virtual void LoadAllService()
        {
            foreach (Transform item in transform)
            {
                _Services.Add(item.gameObject.GetComponent<MonoBehaviour>());
            }
        }
#endif

        protected async void Awake()
        {
            DontDestroyOnLoad(gameObject);
            RegisterServices();
            await BootstrapAsync();
        }

        private void Update()
        {
            if (!IsReady) return;
            Container.TickAll();
        }

        protected virtual void RegisterServices()
        {
            foreach (var service in _Services.OfType<IService>())
            {
                RegisterService(service);
            }
        }

        protected virtual void RegisterService(IService service)
        {
            var type = service.GetType();
            var interfaces = GetMostDerivedInterfaces(type);
            var method = typeof(ServiceContainer).GetMethod(nameof(ServiceContainer.Register));
            if (method == null) return;
            var generic = method.MakeGenericMethod(interfaces.Length == 0 ? type : interfaces[0]);
            generic.Invoke(Container, new object[] { service });
        }

        private Type[] GetMostDerivedInterfaces(Type concreteType)
        {
            var allInterfaces = concreteType.GetInterfaces();
            // Loại bỏ interface nào là "cha" của 1 interface khác trong cùng danh sách
            return allInterfaces
                .Where(candidate => !allInterfaces.Any(other =>
                    other != candidate && candidate.IsAssignableFrom(other)))
                .ToArray();
        }

        protected virtual async Task BootstrapAsync()
        {
            InitializeServices();

            await AsyncInitializeServices();
            IsReady = true;
#if UNITY_EDITOR
            Debug.LogWarning($"[{GetType().Name}] - Ready {IsReady}");
#endif
        }

        protected virtual async Task AsyncInitializeServices()
        {
            var asyncTasks = new List<Task>();
            foreach (var raw in Container.All())
            {
                if (raw is IAsyncInitializable async)
                    asyncTasks.Add(async.InitializeAsync());
            }

            await Task.WhenAll(asyncTasks);
        }

        protected virtual void InitializeServices()
        {
            foreach (var raw in Container.All())
            {
                if (raw is IInitializable sync)
                {
                    sync.Initialize();
                }
            }
        }
    }
}
