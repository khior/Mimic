using System;
using System.Reflection;
using System.Threading.Tasks;

namespace UniversalAdapter;

public sealed class TaskPoolInterfaceAdapter<TImplementation> : IInterfaceAdapter
{
    public T MethodValue<T>(MethodInfo methodInfo, object[] parameters)
    {
        throw new NotImplementedException();
    }

    public void MethodVoid(MethodInfo methodInfo, object[] parameters)
    {
        throw new NotImplementedException();
    }

    public Task<T> MethodValueAsync<T>(MethodInfo methodInfo, object[] parameters)
    {
        throw new NotImplementedException();
    }

    public Task MethodVoidAsync(MethodInfo methodInfo, object[] parameters)
    {
        throw new NotImplementedException();
    }

    public T GetProperty<T>(PropertyInfo propertyInfo)
    {
        throw new NotImplementedException();
    }

    public void SetProperty(PropertyInfo propertyInfo, object parameter)
    {
        throw new NotImplementedException();
    }
}