using System;

internal class LoadProgressState : IState
{
    private readonly GameStateMachine _stateMachine;
    private readonly IPersistentProgressService _progressService;
    private readonly ISaveLoadService _saveLoadProgress;

    public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadProgress)
    {
        _stateMachine = gameStateMachine;
        _progressService = progressService;
        _saveLoadProgress = saveLoadProgress;
    }

    public void Enter()
    {
        LoadProgressOrInitNew();
        _stateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.Level);
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
        new PlayerProgress(initialLevel: "SampleScene");
}
