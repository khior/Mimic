using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UniversalAdapter.Auditing;

public static class AuditingInterfaceExtensions
{
    public static TInt WithAuditing<TInt, TImpl>(this TImpl impl, IServiceProvider serviceProvider) where TImpl : TInt
    {
        return new AuditingInterfaceAdapterFactory().Create<TInt>(impl, serviceProvider.GetRequiredService<ILogger<AuditingInterfaceAdapter<TInt>>>());
    }
}