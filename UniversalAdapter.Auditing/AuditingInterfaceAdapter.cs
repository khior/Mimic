using System.Reflection;
using Microsoft.Extensions.Logging;

namespace UniversalAdapter.Auditing;

public sealed class AuditingInterfaceAdapter<TImplementation>(TImplementation implementation, ILogger<AuditingInterfaceAdapter<TImplementation>> log)
    : IInterfaceAdapter
{
    public T MethodValue<T>(MethodInfo methodInfo, object[] parameters)
    {
        log.LogInformation("Invoking MethodValue: {methodSignature}", methodInfo.GetSignature());
        return (T)methodInfo.Invoke(implementation, parameters)!;
    }

    public void MethodVoid(MethodInfo methodInfo, object[] parameters)
    {
        log.LogInformation("Invoking MethodVoid: {methodSignature}", methodInfo.GetSignature());
        methodInfo.Invoke(implementation, parameters);
    }

    public Task<T> MethodValueAsync<T>(MethodInfo methodInfo, object[] parameters)
    {
        log.LogInformation("Invoking MethodValueAsync: {methodSignature}", methodInfo.GetSignature());
        return (Task<T>)methodInfo.Invoke(implementation, parameters)!;
    }

    public Task MethodVoidAsync(MethodInfo methodInfo, object[] parameters)
    {
        log.LogInformation("Invoking MethodVoidAsync: {methodSignature}", methodInfo.GetSignature());
        return (Task)methodInfo.Invoke(implementation, parameters)!;
    }

    public T GetProperty<T>(PropertyInfo propertyInfo)
    {
        log.LogInformation("Invoking getter: {methodSignature}", propertyInfo.GetSignature());
        return (T)propertyInfo.GetMethod?.Invoke(implementation, [])!;
    }

    public void SetProperty(PropertyInfo propertyInfo, object parameter)
    {
        log.LogInformation("Invoking setter: {methodSignature}", propertyInfo.GetSignature());
        propertyInfo.SetMethod?.Invoke(implementation, [parameter]);
    }
}