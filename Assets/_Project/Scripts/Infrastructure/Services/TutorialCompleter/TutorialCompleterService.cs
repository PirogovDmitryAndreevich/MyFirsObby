public class TutorialCompleterService : ITutorialCompleterService
{
    private readonly IPersistentProgressService _progressService;
    private readonly ISaveLoadService _saveService;

    public TutorialCompleterService(IPersistentProgressService progressService, ISaveLoadService saveService)
    {
        _progressService = progressService;
        _saveService = saveService;
    }

    public void TutorialComplete()
    {
        _progressService.Progress.PlayerState.CompleteTutorial();
        _saveService.SaveProgress();
    }
}