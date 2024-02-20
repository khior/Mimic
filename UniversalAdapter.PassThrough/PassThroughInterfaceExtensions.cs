namespace UniversalAdapter.PassThrough;

public static class PassThroughInterfaceExtensions
{
    public static TInt WithPassThrough<TInt, TImpl>(this TImpl impl) where TImpl : TInt
    {
        return new PassThroughInterfaceAdapterFactory().Create<TInt>(impl);
    }
}