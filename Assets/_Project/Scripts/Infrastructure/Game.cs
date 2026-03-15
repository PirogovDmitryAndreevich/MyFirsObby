using UnityEngine;

public class Game
{
    public static IInputService InputService;
    public GameStateMachine _stateMachine;

    public Game(ICoroutineRunner coroutineRunner)
    {
        RegisterInputService();

        _stateMachine = new GameStateMachine(new SceneLoader(coroutineRunner));
    }

    private void RegisterInputService()
    {
        if (Application.isEditor)
            InputService = new StandaloneInputService();
        else
            InputService = new MobileInputService();
    }
}