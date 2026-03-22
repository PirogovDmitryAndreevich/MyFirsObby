using UnityEngine;
using YG;

public class SaveLoadService : ISaveLoadService
{
    private readonly IPersistentProgressService _progressService;
    private readonly ISavedLists _savedLists;

    public SaveLoadService(IPersistentProgressService progressService ,ISavedLists savedLists)
    {
        _progressService = progressService;
        _savedLists = savedLists;
    }

    public void SaveProgress()
    {
        foreach (ISavedProgress progressWriter in _savedLists.ProgressWriters)
            progressWriter.UpdateProgress(_progressService.Progress);

        YandexGamesSave();
    }

    public PlayerProgress LoadProgress() => 
        YG2.saves.Progress;

    private void YandexGamesSave()
    {
        YG2.saves.Progress = _progressService.Progress;
        YG2.SaveProgress();
    }
}
