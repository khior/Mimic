using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                services.AddTransient(_ => new Jeff().WithPassThrough<IJeff, Jeff>());
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
}