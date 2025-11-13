using System;
using System.Collections.Generic;

namespace HexaSort.DependencyInjection
{
    public class Container
    {
        private readonly Dictionary<Type, object> _instances = new();
        private readonly Dictionary<Type, Func<Container, object>> _factories = new();

        public void RegisterInstance<T>(T instance) where T : class
        {
            _instances[typeof(T)] = instance;
        }

        public void RegisterFactory<T>(Func<Container, T> factory) where T : class
        {
            _factories[typeof(T)] = factory;
        }

        public void RegisterSingleton<TInterface, TImplementation>() where TImplementation : class, TInterface, new()
        {
            _factories[typeof(TInterface)] = container =>
            {
                if (!_instances.ContainsKey(typeof(TInterface)))
                {
                    _instances[typeof(TInterface)] = new TImplementation();
                }

                return _instances[typeof(TInterface)];
            };
        }

        public T Resolve<T>() where T : class
        {
            var type = typeof(T);

            if (_instances.TryGetValue(type, out var instance)) return (T)instance;
            if (!_factories.TryGetValue(type, out var factory)) throw new InvalidOperationException($"Service {type.Name} not registered");

            var created = factory(this);

            if (_instances.ContainsKey(type))
            {
                _instances[type] = created;
            }

            return (T)created;
        }
    }
}