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
        var i = bob.GetInt();
        log.LogInformation("Calling bob with BaseInt:{BaseInt} Result:{int}", bob.BaseInt, i);
        return Task.CompletedTask;
    }
}

public interface IJeff
{
    int BaseInt { get; set; }
    int GetInt(int x);
}

public class Jeff : IJeff
{
    public int BaseInt { get; set; } = 3;

    public int GetInt(int x) => x + BaseInt;
}

public class Bob(IJeff jeff)
{
    public int BaseInt => jeff.BaseInt;
    public int GetInt() => jeff.GetInt(100);
}