using Microsoft.Extensions.Logging;

namespace UniversalAdapter.TestHarness;

public interface ITestHarness
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}

public class TestHarness(Bob bob, ILogger<TestHarness> log) : ITestHarness
{
    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        log.LogInformation("Calling bob: {int}", bob.GetInt());
        return Task.CompletedTask;
    }
}

public interface IJeff
{
    int GetInt();
    int GetInt(int x);
}

public class Jeff : IJeff
{
    public int GetInt() => 123;
    public int GetInt(int x) => x + 3;
}

public class Bob(IJeff jeff)
{
    public int GetInt() => jeff.GetInt(100);
}