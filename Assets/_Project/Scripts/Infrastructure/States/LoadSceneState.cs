using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

internal class LoadSceneState : IPayloaderState<string>
{
    private const string InitialPointTag = "InitialPoint";
    private const string ArsnalPointTag = "ArsenalPoint";
    private const string ConveerPointTag = "ConveerPoint";

    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _curtain;
    private readonly IGameFactory _gameFactory;
    private readonly IUIFactory _uIFactory;
    private readonly IPersistentProgressService _progressService;
    private readonly ISavedLists _savedLists;
    private readonly IStaticDataService _staticData;

    public LoadSceneState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
        LoadingCurtain curtain, IGameFactory gameFactory, IUIFactory uIFactory,
        IPersistentProgressService progressService, ISavedLists savedLists,
        IStaticDataService staticData)
    {
        _stateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _curtain = curtain;
        _gameFactory = gameFactory;
        _uIFactory = uIFactory;
        _progressService = progressService;
        _savedLists = savedLists;
        _staticData = staticData;
    }

    public void Enter(string sceneName)
    {
        _curtain.Show();
        _savedLists.Cleanup();
        _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit() =>
        _curtain.Hide();

    private void OnLoaded()
    {
        InitEnemySpawners();

        InitGameWorld();
        InformProgressReaders();
        _stateMachine.Enter<GameLoopState>();
    }

    private void InitGameWorld()
    {
        Character characterController =
            InitCharacter(at: GameObject.FindWithTag(InitialPointTag).transform);

        IWeaponSelector weaponSelector = InitWeaponSelector(characterController);

        InitConveer();

        Hud hud = InitHud(characterController.Health, weaponSelector);

        ArsenalShopWindow arsenalWindow = InitArsenalShop();

        ArsenalView arsenal = InitArsenal(hud.IndicateUI, arsenalWindow);

        CameraFollow(characterController.CameraFollow);
    }

    private void InitConveer()
    {
        GameObject conveer = _gameFactory.CreateConveer(at: GameObject.FindWithTag(ConveerPointTag));
        Conveer conveerComponent = conveer.GetComponent<Conveer>();

        _gameFactory.CreateConveerManager(conveerComponent);
    }

    private Character InitCharacter(Transform at) =>
        _gameFactory.CreateCharacterController(at: at);

    private ArsenalShopWindow InitArsenalShop() => 
        _uIFactory.CreateArsenalShopWindow();

    private WeaponSelector InitWeaponSelector(Character character) =>
        _gameFactory.CreateWeaponSelector(character);

    private ArsenalView InitArsenal(IIndicateUI indicator, IWindow arsenalWindow) => 
        _gameFactory.CreateArsenal(at: GameObject.FindWithTag(ArsnalPointTag).transform, indicator, arsenalWindow);

    private void InitEnemySpawners()
    {
        string sceneKey = SceneManager.GetActiveScene().name;

        LevelStaticData levelData = _staticData.ForLevel(sceneKey);

        foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
            _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
    }

    private Hud InitHud(CharacterHealth characterHealth, IWeaponSelector weaponSelector) =>
        _uIFactory.CreateHod(YG2.envir.isMobile, weaponSelector, characterHealth);

    private void InformProgressReaders()
    {
        foreach (ISavedProgressReader progressReader in _savedLists.ProgressReaders)        
            progressReader.LoadProgress(_progressService.Progress);        
    }

    private void CameraFollow(Transform cameraFollow)
    {    
        if (Camera.main.TryGetComponent<PlayerCamera>(out PlayerCamera playerCamera))
            playerCamera.SetFollowTransform(cameraFollow);
        else
            Camera.main
                .AddComponent<PlayerCamera>()
                .SetFollowTransform(cameraFollow);
    }
}
