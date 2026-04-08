using YG;

public class SaveLoadService : ISaveLoadService
{
    private readonly IPersistentProgressService _progressService;

    public SaveLoadService(IPersistentProgressService progressService) => 
        _progressService = progressService;

    public void SaveProgress() => 
        YandexGamesSave();

    public PlayerProgress LoadProgress() => 
        YG2.saves.Progress;

    private void YandexGamesSave()
    {
        YG2.saves.Progress = _progressService.Progress;
        YG2.SaveProgress();
    }
}
