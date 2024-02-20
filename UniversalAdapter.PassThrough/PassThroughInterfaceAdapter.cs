using System.Reflection;

namespace UniversalAdapter.PassThrough;

public sealed class PassThroughInterfaceAdapter<TImplementation>(TImplementation implementation) : IInterfaceAdapter
{
    public T MethodValue<T>(MethodInfo methodInfo, object[] parameters)
    {
        return (T)methodInfo.Invoke(implementation, parameters);
    }

    public void MethodVoid(MethodInfo methodInfo, object[] parameters)
    {
        methodInfo.Invoke(implementation, parameters);
    }

    public T GetProperty<T>(PropertyInfo propertyInfo)
    {
        return (T)propertyInfo.GetMethod?.Invoke(implementation, []);
    }

    public void SetProperty(PropertyInfo propertyInfo, object parameter)
    {
        propertyInfo.SetMethod?.Invoke(propertyInfo, [parameter]);
    }

    public Task<T> MethodValueAsync<T>(MethodInfo methodInfo, object[] parameters)
    {
        return (Task<T>)methodInfo.Invoke(implementation, parameters);
    }

    public Task MethodVoidAsync(MethodInfo methodInfo, object[] parameters)
    {
        return (Task)methodInfo.Invoke(implementation, parameters);
    }
}