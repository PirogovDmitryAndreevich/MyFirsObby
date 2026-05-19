using System;
using System.Collections.Generic;

public partial class GameStateMachine
{
    private IExcitableState _activeState;

    private Dictionary<Type, IExcitableState> _states;

    public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain curtain, AllServices services, ICoroutineRunner coroutineRunner)
    {
        _states = new Dictionary<Type, IExcitableState>()
        {
            [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services),

            [typeof(LoadSceneState)] = new LoadSceneState(this, sceneLoader, curtain, services.Single<IGameFactory>(), services.Single<IUIFactory>(),
            services.Single<IPersistentProgressService>(), services.Single<ISavedLists>(), services.Single<IStaticDataService>(),
            coroutineRunner),

            [typeof(LoadProgressState)] = new LoadProgressState(this, services.Single<IPersistentProgressService>(), services.Single<ISaveLoadService>(),
            services.Single<IStaticDataService>()),

            [typeof(GameLoopState)] = new GameLoopState(this, services.Single<IPersistentProgressService>(), coroutineRunner,
            services.Single<ITutorialCompleterService>(), services.Single<IDifficultyService>(), services.Single<IOpenService>())
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
