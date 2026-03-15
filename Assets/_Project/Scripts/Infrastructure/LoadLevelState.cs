using System;
using Unity.VisualScripting;
using UnityEngine;

internal class LoadLevelState : IPayloaderState<string>
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;

    public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
    {
        _gameStateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
    }

    public void Enter(string sceneName) =>
        _sceneLoader.Load(sceneName, OnLoaded);

    public void Exit()
    {
        throw new NotImplementedException();
    }
    private void OnLoaded()
    {
        GameObject character = Instantiate("Character/Character");
        Instantiate("Hud/Hud");

        CameraFollow(character);
    }

    private void CameraFollow(GameObject character)
    {
        PlayerCamera playerCamera;

        if (Camera.main.TryGetComponent<PlayerCamera>(out playerCamera))
            playerCamera.SetFollowTransform(character.transform);
        else
            Camera.main
                .AddComponent<PlayerCamera>()
                .SetFollowTransform(character.transform);
    }

    private GameObject Instantiate(string path)
    {
        GameObject prefab = Resources.Load<GameObject>(path);
        return UnityEngine.Object.Instantiate(prefab);
    }
}