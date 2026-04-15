using System;

public abstract class Transition
{
    protected EnemyStateMachine StateMachine;

    public event Action Transiting;

    protected Transition(EnemyStateMachine stateMachine) => 
        StateMachine = stateMachine;

    public abstract bool IsNeedTransit();

    public virtual void Transit() => 
        Transiting?.Invoke();
}