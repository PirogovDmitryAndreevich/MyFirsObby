using YG;

public class BootstrapState : IState
{
    private const string Initial = "Initial";

    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly AllServices _services;

    public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
    {
        _stateMachine = stateMachine;
        _sceneLoader = sceneLoader;
        _services = services;
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
        _services.RegisterSingle<IPauseService>(new PauseService());        

        _services.RegisterSingle<IBulletsPool>(new BulletsPool(_services.Single<IStaticDataService>()));

        _services.RegisterSingle<IConveyersPool>(new ConveyersPool(_services.Single<IStaticDataService>()));

        _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IPersistentProgressService>()));

        _services.RegisterSingle<IOpenService>(new OpenService(_services.Single<ISaveLoadService>(), _services.Single<IPersistentProgressService>()));

        _services.RegisterSingle<IRewardAdvService>(new RewardAdvService(_services.Single<IPersistentProgressService>(),
            _services.Single<ISaveLoadService>(), _services.Single<IStaticDataService>()));

        _services.RegisterSingle<IDifficultyService>(new DifficultyService(_services.Single<ISaveLoadService>(), _services.Single<IPersistentProgressService>()));

        _services.RegisterSingle<ITutorialCompleterService>
            (new TutorialCompleterService(_services.Single<IPersistentProgressService>(), _services.Single<ISaveLoadService>()));

        _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssets>(),
            _services.Single<IStaticDataService>(), _services.Single<IPersistentProgressService>(),
            _services.Single<ISavedLists>(), _services.Single<ISaveLoadService>(), _services.Single<IInputService>(),
            _services.Single<IPoolService>(), _services.Single<IWaypointChangerService>(),
             _services.Single<IDifficultyService>(), _services.Single<IOpenService>(), _services.Single<IConveyersPool>(),
             _services.Single<IBulletsPool>()));

        _services.RegisterSingle<IUIFactory>(new UIFactory(_services.Single<IAssets>(), _services.Single<IPersistentProgressService>(),
            _services.Single<IStaticDataService>(), _services.Single<IRewardAdvService>(), _services.Single<IPauseService>(), _services.Single<ISaveLoadService>()));
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

