public interface IState : IExcitableState
{
    public void Enter();
}

public interface IPayloaderState<TPayload> : IExcitableState
{
    public void Enter(TPayload payload);
}

public interface IExcitableState
{
    public void Exit();
}