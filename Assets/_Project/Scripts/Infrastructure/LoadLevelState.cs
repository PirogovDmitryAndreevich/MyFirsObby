using Unity.VisualScripting;
using UnityEngine;

internal class LoadLevelState : IPayloaderState<string>
{
    private const string CharacterPath = "Character/Character";
    private const string HudPath = "Hud/Hud";
    private const string InitialPointTag = "InitialPoint";
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _curtain;

    public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain curtain)
    {
        _stateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _curtain = curtain;
    }

    public void Enter(string sceneName)
    {
        _curtain.Show();
        _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit() => 
        _curtain.Hide();

    private void OnLoaded()
    {
        var initialPoint = GameObject.FindWithTag(InitialPointTag);

        GameObject character = Instantiate(CharacterPath, at: initialPoint.transform.position);
        Instantiate(HudPath);

        CameraFollow(character);

        _stateMachine.Enter<GameLoopState>();
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
    private GameObject Instantiate(string path, Vector3 at)
    {
        GameObject prefab = Resources.Load<GameObject>(path);
        return UnityEngine.Object.Instantiate(prefab, at, Quaternion.identity);
    }
}