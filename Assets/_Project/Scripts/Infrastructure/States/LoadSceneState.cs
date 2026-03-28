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
    private readonly IPersistentProgressService _progressService;
    private readonly ISavedLists _savedLists;
    private readonly IStaticDataService _staticData;
    private readonly ISaveLoadService _saveLoadService;

    public LoadSceneState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
        LoadingCurtain curtain, IGameFactory gameFactory,
        IPersistentProgressService progressService, ISavedLists savedLists,
        IStaticDataService staticData, ISaveLoadService saveLoadService)
    {
        _stateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _curtain = curtain;
        _gameFactory = gameFactory;
        _progressService = progressService;
        _savedLists = savedLists;
        _staticData = staticData;
        _saveLoadService = saveLoadService;
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
        CharacterController characterController =
            InitCharacter(at: GameObject.FindWithTag(InitialPointTag).transform);

        IInventory inventory = InitInventory(characterController);

        InitConveer();

        Hud hud = InitHud(characterController.Health);

        ArsenalView arsenal = InitArsenal(inventory, hud.IndicateUI);

        InitArsenalShop(inventory);

        CameraFollow(characterController.CameraFollow);
    }

    private void InitConveer()
    {
        GameObject conveer = _gameFactory.CreateConveer(at: GameObject.FindWithTag(ConveerPointTag));
        Conveer conveerComponent = conveer.GetComponent<Conveer>();

        _gameFactory.CreateConveerManager(conveerComponent);
    }

    private CharacterController InitCharacter(Transform at) =>
        _gameFactory.CreateCharacterController(at: at);

    private void InitArsenalShop(IInventory inventory)
    {
        GameObject shop = _gameFactory.CreateArsenalShop();
        ArsenalShop arsenalShop = shop.GetComponent<ArsenalShop>();
        arsenalShop.Construct(inventory);
    }

    private Inventory InitInventory(CharacterController character) =>
        _gameFactory.CreateInventory(character);

    private ArsenalView InitArsenal(IInventory inventory, IIndicateUI indicator) => 
        _gameFactory.CreateArsenal(at: GameObject.FindWithTag(ArsnalPointTag).transform, inventory, indicator);

    private void InitEnemySpawners()
    {
        string sceneKey = SceneManager.GetActiveScene().name;

        LevelStaticData levelData = _staticData.ForLevel(sceneKey);

        foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
            _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
    }

    private Hud InitHud(CharacterHealth character)
    {
        Hud hud = _gameFactory.CreateHod(YG2.envir.isMobile);
        hud.HealthUI.Construct(character);

        return hud;
    }

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
