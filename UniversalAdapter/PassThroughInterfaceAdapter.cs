using System.Reflection;

namespace UniversalAdapter;

public sealed class PassThroughInterfaceAdapter<T>(T implementation) : IInterfaceAdapter
{
    public object Method(MethodInfo methodInfo, object[] parameters)
    {
        return methodInfo.Invoke(implementation, parameters);
    }

    public object GetProperty(PropertyInfo propertyInfo)
    {
        return propertyInfo.GetMethod?.Invoke(implementation, []);
    }

    public object SetProperty(PropertyInfo propertyInfo, object parameter)
    {
        return propertyInfo.SetMethod?.Invoke(implementation, [parameter]);
    }
}