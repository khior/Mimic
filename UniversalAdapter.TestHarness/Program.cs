using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace UniversalAdapter.TestHarness;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        await Host
            .CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddTransient<ITestHarness, TestHarness>();
                services.AddTransient(x => new Jeff().WithPassThrough<IJeff, IJeff>().WithAuditing<IJeff, IJeff>(x));
                services.AddTransient<Bob>();

                services.AddHostedService<TestHarnessHostedService>();
            })
            .RunConsoleAsync(options => options.SuppressStatusMessages = true);
    }
}

public static class InterfaceExtensions
{
    public static TInt WithPassThrough<TInt, TImpl>(this TImpl impl) where TImpl : TInt
    {
        return new PassThroughInterfaceAdapterFactory().Create<TInt>(impl);
    }
    
    public static TInt WithAuditing<TInt, TImpl>(this TImpl impl, IServiceProvider serviceProvider) where TImpl : TInt
    {
        return new AuditingInterfaceAdapterFactory().Create<TInt>(impl, serviceProvider.GetRequiredService<ILogger<AuditingInterfaceAdapter<TInt>>>());
    }
}