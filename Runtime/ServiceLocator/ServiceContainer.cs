using System;
using System.Collections.Generic;
using UnityEngine;

namespace Eagle
{
    public sealed class ServiceContainer
    {
        private readonly Dictionary<Type, object> _services = new();
        private readonly List<ITickable> _tickables = new();
        private readonly object _lock = new();

        public void Register<T>(T service) where T : class
        {
            Type type = typeof(T);
            lock (_lock)
            {
                if (_services.ContainsKey(type))
                {
#if UNITY_EDITOR
                    Debug.LogWarning($"[ServiceContainer] - {type.Name} đã được register, sẽ ghi đè.");
#endif
                }

                _services[type] = service;

                if (service is ITickable tickable)
                {
                    _tickables.Add(tickable);
                }
            }
        }

        public T Get<T>() where T : class, IService
        {
            lock (_lock)
            {
                if (_services.TryGetValue(typeof(T), out object service))
                {
                    return (T)service;
                }
            }

            throw new InvalidOperationException($"[ServiceContainer] - Service {typeof(T)} chưa được register.");
        }

        public bool TryGet<T>(out T service) where T : class, IService
        {
            lock (_lock)
            {
                if (_services.TryGetValue(typeof(T), out object raw))
                {
                    service = (T)raw;
                    return true;
                }
            }

            service = null;
            return false;
        }

        public bool IsRegistered<T>() where T : class, IService
        {
            lock (_lock)
            {
                return _services.ContainsKey(typeof(T));
            }
        }

        public void Unregister<T>() where T : class, IService
        {
            lock (_lock)
            {
                _services.Remove(typeof(T));
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                foreach (KeyValuePair<Type, object> kv in _services)
                {
                    if (kv.Value is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }

                _services.Clear();
            }
        }

        public void TickAll()
        {
            foreach (ITickable t in _tickables)
            {
                t.Tick();
            }
        }

        public IReadOnlyCollection<object> All()
        {
            lock (_lock)
            {
                return _services.Values;
            }
        }
    }
}
