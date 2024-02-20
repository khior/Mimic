using System.Reflection;
using System.Threading.Tasks;

namespace UniversalAdapter
{
    public interface IInterfaceAdapter
    {
        T MethodValue<T>(MethodInfo methodInfo, object[] parameters);
        void MethodVoid(MethodInfo methodInfo, object[] parameters);
        Task<T> MethodValueAsync<T>(MethodInfo methodInfo, object[] parameters);
        Task MethodVoidAsync(MethodInfo methodInfo, object[] parameters);
        T GetProperty<T>(PropertyInfo propertyInfo);
        void SetProperty(PropertyInfo propertyInfo, object parameter);
    }
}