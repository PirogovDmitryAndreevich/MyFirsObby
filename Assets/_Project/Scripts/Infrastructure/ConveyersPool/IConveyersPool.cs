public interface IConveyersPool : IService
{
    ConveyerResource Get(ConveyerType type);
    void Return(ConveyerResource resource);
}