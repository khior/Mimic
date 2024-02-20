using System.Reflection;
using System.Threading.Tasks;

namespace UniversalAdapter.Tests;

public class TestClass
{
    public int Jeff(IInterfaceAdapter adapter, MethodInfo methodInfo, object[] parameters)
    {
        return adapter.MethodValue<int>(methodInfo, parameters);
    }
}