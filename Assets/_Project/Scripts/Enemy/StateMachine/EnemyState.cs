public abstract class EnemyState
{
    protected Transition[] Transitions;
    protected Enemy Enemy;

    public EnemyState(EnemyStateMachine stateMachine, Enemy enemy) =>
        Enemy = enemy;

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void Update() { }

    public virtual void TryTransit()
    {
        foreach (Transition transition in Transitions)
        {
            if (transition.IsNeedTransit())
            {
                transition.Transit();
                return;
            }
        }
    }
}
