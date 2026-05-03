internal class LoadProgressState : IState
{
    private readonly GameStateMachine _stateMachine;
    private readonly IPersistentProgressService _progressService;
    private readonly ISaveLoadService _saveLoadProgress;
    private readonly IStaticDataService _staticData;
    private readonly IWeaponCreator _weaponCreator;

    public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadProgress,
        IStaticDataService staticData, IWeaponCreator weaponCreator)
    {
        _stateMachine = gameStateMachine;
        _progressService = progressService;
        _saveLoadProgress = saveLoadProgress;
        _staticData = staticData;
        _weaponCreator = weaponCreator;
    }

    public void Enter()
    {
        LoadProgressOrInitNew();

        foreach (WeaponType weaponType in _progressService.Progress.Weapons.OpenedWeapons)
            _weaponCreator.AddNewWeapon(weaponType);

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
