public interface IConveyersPool : IService
{
    ConveerResource Get(ConveyerType type);
    void Return(ConveerResource resource);
}