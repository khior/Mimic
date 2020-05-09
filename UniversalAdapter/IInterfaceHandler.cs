using System.Reflection;

namespace UniversalAdapter
{
    /// <summary>
    /// I
    /// </summary>
    public interface IInterfaceHandler
    {
        object Method(MethodInfo methodInfo, object[] parameters);
        object GetProperty(PropertyInfo propertyInfo);
        object SetProperty(PropertyInfo propertyInfo, object parameter);
    }
}