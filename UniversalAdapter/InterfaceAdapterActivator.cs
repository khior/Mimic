using System;
using System.Collections.Generic;

namespace UniversalAdapter
{
    internal sealed class InterfaceAdapterActivator
    {
        internal InterfaceAdapterActivator(Type interfaceAdapterType, IReadOnlyCollection<object> constructorArguments)
        {
            InterfaceAdapterType = interfaceAdapterType;
            ConstructorArguments = constructorArguments;
        }

        internal Type InterfaceAdapterType { get; }
        internal IReadOnlyCollection<object> ConstructorArguments { get; }

        internal object CreateInstance(IInterfaceHandler adapter)
        {
            var paramsArray = new List<object>(ConstructorArguments) {adapter}.ToArray();
            
            return Activator.CreateInstance(InterfaceAdapterType, paramsArray);
        }
    }
}