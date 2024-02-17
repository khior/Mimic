using System;
using System.Collections.Generic;

namespace UniversalAdapter
{
    internal sealed class InterfaceAdapterActivator
    {
        internal InterfaceAdapterActivator(Type interfaceAdapterType, List<object> constructorArguments)
        {
            _interfaceAdapterType = interfaceAdapterType;
            _constructorArguments = constructorArguments;
        }

        private readonly Type _interfaceAdapterType;
        private readonly List<object> _constructorArguments;

        internal object CreateInstance(IInterfaceAdapter adapter)
        {
            _constructorArguments.Add(adapter);
            
            var paramsArray = _constructorArguments.ToArray();
            
            return Activator.CreateInstance(_interfaceAdapterType, paramsArray);
        }
    }
}