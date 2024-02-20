using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UniversalAdapter.Auditing;
using UniversalAdapter.PassThrough;

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