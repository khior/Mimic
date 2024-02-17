using Microsoft.Extensions.Hosting;

namespace UniversalAdapter.TestHarness;

public sealed class TestHarnessHostedService(ITestHarness testHarness, IHostApplicationLifetime appLifetime)
    : BackgroundService
{
    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await Task.Yield();
        await testHarness.ExecuteAsync(cancellationToken);
        appLifetime.StopApplication();
    }
}