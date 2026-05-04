internal class LoadProgressState : IState
{
    private readonly GameStateMachine _stateMachine;
    private readonly IPersistentProgressService _progressService;
    private readonly ISaveLoadService _saveLoadProgress;
    private readonly IStaticDataService _staticData;

    public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadProgress,
        IStaticDataService staticData)
    {
        _stateMachine = gameStateMachine;
        _progressService = progressService;
        _saveLoadProgress = saveLoadProgress;
        _staticData = staticData;
    }

    public void Enter()
    {
        LoadProgressOrInitNew();

        _stateMachine.Enter<LoadSceneState, string>(_progressService.Progress.MainScene);
    }

    public void Exit()
    {
        
    }

    private void LoadProgressOrInitNew() => 
        _progressService.Progress =
            _saveLoadProgress.
            LoadProgress()
            ?? NewProgress();

    private PlayerProgress NewProgress() =>
        new PlayerProgress(_staticData);
}
