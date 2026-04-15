using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
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
    private readonly IUIFactory _uiFactory;
    private readonly IPersistentProgressService _progressService;
    private readonly ISavedLists _savedLists;
    private readonly IStaticDataService _staticData;

    private LoadedScene _loadedScene;

    public LoadSceneState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
        LoadingCurtain curtain, IGameFactory gameFactory, IUIFactory uIFactory,
        IPersistentProgressService progressService, ISavedLists savedLists,
        IStaticDataService staticData)
    {
        _stateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _curtain = curtain;
        _gameFactory = gameFactory;
        _uiFactory = uIFactory;
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
        InitGameWorld();
        InformProgressReaders();
        _stateMachine.Enter<GameLoopState, LoadedScene>(_loadedScene);
    }

    private void InitGameWorld()
    {
        List<SpawnPoint> enemySpawns = InitEnemySpawns();

        InitWaypoints();

        Character characterController =
            InitCharacter(at: GameObject.FindWithTag(InitialPointTag).transform);

        IWeaponSelector weaponSelector = InitWeaponSelector(characterController);

        Hud hud = InitHud(characterController.Health, weaponSelector);

        ArsenalShopWindow arsenalWindow = InitArsenalShop();
        ConveerShop conveerShop = InitConveerShop();

        ConveerInitializer conveerView = InitConveerView(hud.IndicateUI, conveerShop);
        ArsenalInitializer arsenal = InitArsenal(hud.IndicateUI, arsenalWindow);

        CameraFollow(characterController.CameraFollow);

        _loadedScene = new LoadedScene(characterController, conveerView, arsenal, enemySpawns);
    }

    private ConveerShop InitConveerShop() => 
        _uiFactory.CreateConveerShop();

    private ConveerInitializer InitConveerView(IIndicateUI indicator, IWindow conveerShopWindow) => 
        _gameFactory.CreateConveerView(at: GameObject.FindWithTag(ConveerPointTag), indicator, conveerShopWindow);

    private Character InitCharacter(Transform at) =>
        _gameFactory.CreateCharacterController(at: at);

    private ArsenalShopWindow InitArsenalShop() => 
        _uiFactory.CreateArsenalShopWindow();

    private WeaponSelector InitWeaponSelector(Character character) =>
        _gameFactory.CreateWeaponSelector(character);

    private ArsenalInitializer InitArsenal(IIndicateUI indicator, IWindow arsenalWindow) => 
        _gameFactory.CreateArsenal(at: GameObject.FindWithTag(ArsnalPointTag).transform, indicator, arsenalWindow);

    private List<SpawnPoint> InitEnemySpawns()
    {
        List<SpawnPoint> enemySpawns = new();

        string sceneKey = SceneManager.GetActiveScene().name;

        LevelStaticData levelData = _staticData.ForLevel(sceneKey);

        foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
        {
            SpawnPoint spawn = _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
            enemySpawns.Add(spawn);
        }

        return enemySpawns;
    }

    private void InitWaypoints()
    {
        string sceneKey = SceneManager.GetActiveScene().name;

        LevelStaticData levelData = _staticData.ForLevel(sceneKey);

        foreach (EnemyWaypointData waypointData in levelData.EnemyWaypoints)        
            _gameFactory.CreateWaypoint(at: waypointData.Position);
    }

    private Hud InitHud(CharacterHealth characterHealth, IWeaponSelector weaponSelector) =>
        _uiFactory.CreateHod(YG2.envir.isMobile, weaponSelector, characterHealth);

    private void InformProgressReaders()
    {
        foreach (ISavedProgressReader progressReader in _savedLists.ProgressReaders)        
            progressReader.LoadProgress(_progressService.Progress);        
    }

    private void CameraFollow(Transform cameraFollow)
    {
        CinemachineFreeLook camera = _gameFactory.CreateCamera();

        camera.Follow = cameraFollow;
        camera.LookAt = cameraFollow;
    }
}
