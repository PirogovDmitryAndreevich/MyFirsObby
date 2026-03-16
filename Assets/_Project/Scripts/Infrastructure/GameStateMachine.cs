using System;
using System.Collections.Generic;

public partial class GameStateMachine
{
    private IExcitableState _activeState;

    private Dictionary<Type, IExcitableState> _states;

    public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain curtain)
    {
        _states = new Dictionary<Type, IExcitableState>()
        {
            [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader),
            [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, curtain),
            [typeof(GameLoopState)] = new GameLoopState(this)
        };
    }

    public void Enter<TState>() where TState : class, IState
    {
        IState state = ChangeState<TState>();
        state.Enter();
    }

    public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloaderState<TPayload>
    {
        TState state = ChangeState<TState>();
        state.Enter(payload);
    }
    
    private TState GetState<TState>() where TState : class, IExcitableState =>
        _states[typeof(TState)] as TState;

    private TState ChangeState<TState>() where TState : class, IExcitableState
    {
        _activeState?.Exit();

        TState state = GetState<TState>();
        _activeState = state;
        return state;
    }
}

internal class GameLoopState : IState
{
    public GameLoopState(GameStateMachine stateMachine)
    {
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }
}