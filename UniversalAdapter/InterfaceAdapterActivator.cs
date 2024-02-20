using System;
using System.Collections.Generic;

namespace UniversalAdapter
{
    internal sealed class InterfaceAdapterActivator
    {
        internal InterfaceAdapterActivator(Type interfaceAdapterType, IReadOnlyList<object> constructorArguments)
        {
            _interfaceAdapterType = interfaceAdapterType;
            _args = new object[constructorArguments.Count + 1];
            for (var index = 0; index < constructorArguments.Count; index++)
            {
                _args[index] = constructorArguments[index];
            }
        }

        private readonly Type _interfaceAdapterType;
        private readonly object[] _args;

        internal object CreateInstance(IInterfaceAdapter adapter)
        {
            try
            {
                _args[^1] = adapter;
                return Activator.CreateInstance(_interfaceAdapterType, _args);
            }
            finally
            {
                _args[^1] = null;
            }
        }
    }
}