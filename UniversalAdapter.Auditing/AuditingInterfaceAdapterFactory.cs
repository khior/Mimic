using Microsoft.Extensions.Logging;

namespace UniversalAdapter.Auditing;

public sealed class AuditingInterfaceAdapterFactory
{
    private readonly UniversalAdapterFactory _factory = new();

    public T Create<T>(T adapter, ILogger<AuditingInterfaceAdapter<T>> log)
    {
        return _factory.Create<T>(adapter: new AuditingInterfaceAdapter<T>(adapter, log));
    }
}