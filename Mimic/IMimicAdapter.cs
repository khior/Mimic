using System.Reflection;

namespace Mimic
{
    public interface IMimicAdapter
    {
        object Method(MethodInfo methodInfo, object[] parameters);
        object GetProperty(PropertyInfo propertyInfo);
        object SetProperty(PropertyInfo propertyInfo, object parameter);
    }
}