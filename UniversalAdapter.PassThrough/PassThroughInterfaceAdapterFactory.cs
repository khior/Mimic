namespace UniversalAdapter.PassThrough;

public sealed class PassThroughInterfaceAdapterFactory
{
    private readonly UniversalAdapterFactory _factory = new();

    public T Create<T>(T adapter)
    {
        return _factory.Create<T>(adapter: new PassThroughInterfaceAdapter<T>(adapter));
    }
}