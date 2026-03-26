using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

internal class LoadSceneState : IPayloaderState<string>
{
    private const string InitialPointTag = "InitialPoint";
    private const string ArsnalPointTag = "ArsenalPoint";

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
        GameObject character = InitCharacter();
        CharacterController characterController = character.GetComponent<CharacterController>();
        CharacterHealth characterHealth = character.GetComponent<CharacterHealth>();

        GameObject hud = InitHud(characterHealth);
        InventoryView inventoryView = hud.GetComponentInChildren<InventoryView>();

        InitInventory(characterController, inventoryView);

        InitArsenal();

        CameraFollow(character);
    }

    private GameObject InitCharacter() =>
        _gameFactory.CreateCharacter(at: GameObject.FindWithTag(InitialPointTag));

    private void InitInventory(CharacterController character , InventoryView inventoryView)
    {
        Inventory inventory = _gameFactory.CreateInventory();
        inventory.Construct(character, _staticData.ForWeapons(), inventoryView, _saveLoadService);
    }

    private void InitArsenal() => 
        _gameFactory.CreateArsenal(at: GameObject.FindWithTag(ArsnalPointTag));

    private void InitEnemySpawners()
    {
        string sceneKey = SceneManager.GetActiveScene().name;

        LevelStaticData levelData = _staticData.ForLevel(sceneKey);

        foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
            _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
    }

    private GameObject InitHud(CharacterHealth character)
    {
        GameObject hud = _gameFactory.CreateHod(YG2.envir.isMobile);

        hud.GetComponentInChildren<ActorUI>().Construct(character);

        return hud;
    }

    private void InformProgressReaders()
    {
        foreach (ISavedProgressReader progressReader in _savedLists.ProgressReaders)        
            progressReader.LoadProgress(_progressService.Progress);        
    }

    private void CameraFollow(GameObject character)
    {
        Transform cameraFollow = character.GetComponent<CharacterController>().CameraFollow;
        PlayerCamera playerCamera;

        if (Camera.main.TryGetComponent<PlayerCamera>(out playerCamera))
            playerCamera.SetFollowTransform(cameraFollow);
        else
            Camera.main
                .AddComponent<PlayerCamera>()
                .SetFollowTransform(cameraFollow);
    }
}
