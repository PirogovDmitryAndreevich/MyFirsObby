public class TargetReachedTransition : ReachedTransition
{
    public TargetReachedTransition(EnemyStateMachine stateMachine, Enemy enemy)
        : base(stateMachine, enemy) { }

    public override void Transit()
    {
        base.Transit();
        StateMachine.ChangeState<AttackState>();
    }
}
