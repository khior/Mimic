using System.Reflection;

namespace UniversalAdapter;

public sealed class PassThroughInterfaceAdapter<T>(T implementation) : IInterfaceAdapter
{
    public object Method(MethodInfo methodInfo, object[] parameters) =>
        methodInfo.Invoke(implementation, parameters);

    public object GetProperty(PropertyInfo propertyInfo) =>
        propertyInfo.GetMethod.Invoke(implementation, []);

    public object SetProperty(PropertyInfo propertyInfo, object parameter) =>
        propertyInfo.SetMethod.Invoke(implementation, [parameter]);
}