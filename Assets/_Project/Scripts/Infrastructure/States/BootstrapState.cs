using YG;

public class BootstrapState : IState
{
    private const string Initial = "Initial";

    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly AllServices _services;
    private readonly ICoroutineRunner _coroutineRunner;

    public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services, ICoroutineRunner coroutineRunner)
    {
        _stateMachine = stateMachine;
        _sceneLoader = sceneLoader;
        _services = services;
        _coroutineRunner = coroutineRunner;
        RegisterServices();
    }

    public void Enter()
    {
        _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);
    }

    private void EnterLoadLevel() =>
        _stateMachine.Enter<LoadProgressState>();

    public void Exit()
    {

    }

    private void RegisterServices()
    {
        RegisterStaticData();
        _services.RegisterSingle(RegisterInputService());
        _services.RegisterSingle<IAssets>(new AssetProvider());
        _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
        _services.RegisterSingle<ISavedLists>(new SavedLists());
        _services.RegisterSingle<IWaypointChangerService>(new WaypointChangerService());

        _services.RegisterSingle<IPoolService>(new PoolService());

        _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IPersistentProgressService>()));

        _services.RegisterSingle<IRewardAdvService>(new RewardAdvService(_services.Single<IPersistentProgressService>(),
            _services.Single<ISaveLoadService>(), _services.Single<IStaticDataService>()));

        _services.RegisterSingle<IDifficultyService>(new DifficultyService(_services.Single<ISaveLoadService>(), _services.Single<IPersistentProgressService>()));

        _services.RegisterSingle<ITutorialCompleterService>
            (new TutorialCompleterService(_services.Single<IPersistentProgressService>(), _services.Single<ISaveLoadService>()));

        _services.RegisterSingle<IWeaponUpdaterService>(new WeaponUpdaterService(_services.Single<IPersistentProgressService>(), _services.Single<IStaticDataService>(),
            _services.Single<ISaveLoadService>()));

        _services.RegisterSingle<IConveerUpdaterService>(new ConveerUpdaterService(_services.Single<IPersistentProgressService>(), _services.Single<IStaticDataService>(),
            _services.Single<ISaveLoadService>()));

        _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssets>(),
            _services.Single<IStaticDataService>(), _services.Single<IPersistentProgressService>(),
            _services.Single<ISavedLists>(), _services.Single<ISaveLoadService>(), _services.Single<IInputService>(),
            _services.Single<IConveerUpdaterService>(), _services.Single<IPoolService>(), _services.Single<IWaypointChangerService>(),
            _coroutineRunner, _services.Single<IDifficultyService>()));

        _services.RegisterSingle<IUIFactory>(new UIFactory(_services.Single<IAssets>(), _services.Single<IPersistentProgressService>(),
            _services.Single<IStaticDataService>(), _services.Single<IWeaponUpdaterService>(), _services.Single<IConveerUpdaterService>(),
            _services.Single<IRewardAdvService>()));
    }

    private void RegisterStaticData()
    {
        IStaticDataService staticData = new StaticDataService();
        staticData.Load();
        _services.RegisterSingle(staticData);
    }

    private IInputService RegisterInputService()
    {
        if (YG2.envir.isDesktop)
            return new StandaloneInputService();
        else
            return new MobileInputService();
    }
}

