using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using YG;

internal class LoadSceneState : IPayloaderState<string>
{
    private const string BuildPointTag = "BuildInitialPoint";
    private const string LeaderBoardTag = "LeaderBoardInitialPoint";

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
        LevelStaticData levelData = GetLevelData();

        CreateLeaderBoard();

        RewardAdvPopup rewardPopup = CreateRewardAdvPopup();

        List<SpawnPoint> enemySpawns = InitEnemySpawns(levelData);

        InitWaypoints(levelData);

        Build build = InitBuild(levelData, rewardPopup);

        Character characterController =
            InitCharacter(at: build.PlayerInitialPoint);

        ATM atm = CreateATM(build.ATMInitialPoint);

        IWeaponSelector weaponSelector = InitWeaponSelector(characterController);

        Hud hud = InitHud(characterController.Health, characterController.Shooter, weaponSelector);

        ArsenalShopWindow arsenalWindow = InitArsenalShop();
        ConveerShop conveerShop = InitConveerShop();

        ConveerInitializer conveerView = InitConveerView(build.ConveerInitialPoint, atm, hud.IndicateUI, conveerShop);
        ArsenalInitializer arsenal = InitArsenal(build.ArsenalInitialPoint, hud.IndicateUI, arsenalWindow);

        CameraFollow(characterController.CameraFollow);

        _loadedScene = new LoadedScene(levelData, characterController, conveerView, arsenal, enemySpawns, build, hud, atm);
    }

    private RewardAdvPopup CreateRewardAdvPopup() =>
        _uiFactory.CreateRewardAdvPopup();

    private void CreateLeaderBoard() => 
        _gameFactory.CreateLeaderBoard(GameObject.FindWithTag(LeaderBoardTag).transform);

    private ATM CreateATM(Transform ATMInitialPoint) =>
        _gameFactory.CreateATM(at: ATMInitialPoint);

    private LevelStaticData GetLevelData()
    {
        string sceneKey = SceneManager.GetActiveScene().name;
        LevelStaticData levelData = _staticData.ForLevel(sceneKey);

        return levelData;
    }

    private Build InitBuild(LevelStaticData levelData, RewardAdvPopup rewardAdvPopup) =>
        _gameFactory.CreateBuild(at: GameObject.FindWithTag(BuildPointTag).transform, levelData, rewardAdvPopup);

    private ConveerShop InitConveerShop() =>
        _uiFactory.CreateConveerShop();

    private ConveerInitializer InitConveerView(Transform at, ATM atm, IIndicateUI indicator, IWindow conveerShopWindow) =>
        _gameFactory.CreateConveerView(at: at, atm, indicator, conveerShopWindow);

    private Character InitCharacter(Transform at) =>
        _gameFactory.CreateCharacterController(at: at);

    private ArsenalShopWindow InitArsenalShop() =>
        _uiFactory.CreateArsenalShopWindow();

    private WeaponSelector InitWeaponSelector(Character character) =>
        _gameFactory.CreateWeaponSelector(character);

    private ArsenalInitializer InitArsenal(Transform at, IIndicateUI indicator, IWindow arsenalWindow)
    {
        string sceneKey = SceneManager.GetActiveScene().name;
        LevelStaticData levelData = _staticData.ForLevel(sceneKey);

        return _gameFactory.CreateArsenal(at: at, levelData, indicator, arsenalWindow);
    }

    private List<SpawnPoint> InitEnemySpawns(LevelStaticData levelData)
    {
        List<SpawnPoint> enemySpawns = new();

        foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
        {
            SpawnPoint spawn = _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
            enemySpawns.Add(spawn);
        }

        return enemySpawns;
    }

    private void InitWaypoints(LevelStaticData levelData)
    {
        foreach (EnemyWaypointData waypointData in levelData.EnemyWaypoints)
            _gameFactory.CreateWaypoint(at: waypointData.Position);
    }

    private Hud InitHud(CharacterHealth characterHealth, Shooter shooter, IWeaponSelector weaponSelector) =>
        _uiFactory.CreateHod(YG2.envir.isMobile, weaponSelector, characterHealth, shooter);

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
