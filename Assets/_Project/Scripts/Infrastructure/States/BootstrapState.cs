using UnityEngine;
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

        _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IPersistentProgressService>()));

        _services.RegisterSingle<IWeaponUpdaterService>(new WeaponUpdaterService(_services.Single<IPersistentProgressService>(), _services.Single<IStaticDataService>(),
            _services.Single<ISaveLoadService>()));

        _services.RegisterSingle<IConveerUpdaterService>(new ConveerUpdaterService(_services.Single<IPersistentProgressService>(), _services.Single<IStaticDataService>(),
            _services.Single<ISaveLoadService>()));

        _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssets>(),
            _services.Single<IStaticDataService>(), _services.Single<IPersistentProgressService>(),
            _services.Single<ISavedLists>(), _services.Single<ISaveLoadService>(), _services.Single<IInputService>(),
            _services.Single<IConveerUpdaterService>()));

        _services.RegisterSingle<IUIFactory>(new UIFactory(_services.Single<IAssets>(), _services.Single<IPersistentProgressService>(),
            _services.Single<IStaticDataService>(), _services.Single<IWeaponUpdaterService>(), _services.Single<IConveerUpdaterService>()));
    }

    private void RegisterStaticData()
    {
        IStaticDataService staticData = new StaticDataService();
        staticData.Load();
        _services.RegisterSingle(staticData);
    }

    private IInputService RegisterInputService()
    {
#if UNITY_EDITOR
        if (Application.isEditor)
            return new StandaloneInputService();
        else
            return new MobileInputService();
#else

        if (YG2.envir.isDesktop)
            return new StandaloneInputService();
        else
            return new MobileInputService();
#endif
    }
}

