public class ConveerUpdaterService : IConveerUpdaterService
{
    private IPersistentProgressService _progress;
    private IStaticDataService _staticData;
    private ISaveLoadService _saveService;

    public ConveerUpdaterService(IPersistentProgressService progress, IStaticDataService staticData, ISaveLoadService saveService)
    {
        _progress = progress;
        _staticData = staticData;
        _saveService = saveService;
    }

    public void UnblockConveer(ConveerLineProgress progressLine)
    {
        if (progressLine.IsOpened)
            return;

        ConveerStaticInfo data = _staticData.ForConveers(progressLine.Numbers);

        if (_progress.Progress.PlayerState.Cash >= data.UnblockPrice)
        {
            progressLine.Unblock();
            _progress.Progress.PlayerState.SpendCash(data.UnblockPrice);
            _saveService.SaveProgress();
        }
    }

    public void UpdateConveer(ConveerLineProgress progressLine, ConveerUpdateType type)
    {
        if (!progressLine.CanUpgrade(type))
            return;

        ConveerStaticInfo data = _staticData.ForConveers(progressLine.Numbers);

        int price = data.GetNextLevelPrice(progressLine.GetLevel(type), type);

        if (_progress.Progress.PlayerState.Cash >= price)
        {
            progressLine.UpdateLevel(type);
            _progress.Progress.PlayerState.SpendCash(price);
            _saveService.SaveProgress();
        }
    }
}