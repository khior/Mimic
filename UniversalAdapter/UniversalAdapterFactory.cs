using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace UniversalAdapter
{
    public interface IUniversalAdapterFactory
    {
        /// <summary>
        /// Creates an adapter for the given interface, using the handler provided.
        /// </summary>
        /// <param name="interfaceType">The Type of the interface to implement</param>
        /// <param name="adapter">The handler that will be invoked by the adapter</param>
        /// <returns>An adapter implementation for the given interface Type</returns>
        object Create(Type interfaceType, IInterfaceAdapter adapter);

        /// <summary>
        /// Creates an adapter for the given interface, using the handler provided.
        /// </summary>
        /// <typeparam name="T">The Type of the interface to implement</typeparam>
        /// <param name="adapter">The handler that will be invoked by the adapter</param>
        /// <returns>An adapter implementation for the given interface T</returns>
        T Create<T>(IInterfaceAdapter adapter);
    }

    public sealed class UniversalAdapterFactory : IUniversalAdapterFactory
    {
        private readonly Dictionary<Type, InterfaceAdapterActivator> _activatorMap;
        private readonly InterfaceAdapterActivatorFactory _activatorFactory;

        public UniversalAdapterFactory()
        {
            var id = Guid.NewGuid().ToString("N");

            var module =
                AssemblyBuilder
                    .DefineDynamicAssembly(new AssemblyName(id), AssemblyBuilderAccess.Run)
                    .DefineDynamicModule("UniversalAdapters");

            _activatorFactory = new InterfaceAdapterActivatorFactory(module);
            _activatorMap = new Dictionary<Type, InterfaceAdapterActivator>();
        }

        /// <inheritdoc />
        public object Create(Type interfaceType, IInterfaceAdapter adapter)
        {
            if (!_activatorMap.TryGetValue(interfaceType, out var activator))
            {
                activator = _activatorFactory.Create(interfaceType);

                _activatorMap.Add(interfaceType, activator);
            }

            return activator.CreateInstance(adapter);
        }
        
        /// <inheritdoc />
        public T Create<T>(IInterfaceAdapter adapter)
        {
            return (T)Create(typeof(T), adapter);
        }
    }
}