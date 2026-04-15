public class WayPointReachedTransition : ReachedTransition
{
    public WayPointReachedTransition(EnemyStateMachine stateMachine, Enemy enemy)
        : base(stateMachine, enemy) { }

    public override void Transit()
    {
        base.Transit();
        StateMachine.ChangeState<IdleState>();
    }
}
