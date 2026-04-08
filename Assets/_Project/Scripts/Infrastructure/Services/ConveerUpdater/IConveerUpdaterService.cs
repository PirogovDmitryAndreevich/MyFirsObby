public interface IConveerUpdaterService : IService
{
    void UnblockConveer(ConveerLineProgress progressLine);
    void UpdateConveer(ConveerLineProgress progressLine, ConveerUpdateType type);
}