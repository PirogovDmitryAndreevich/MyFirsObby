public class TargetReachedTransition : Transition
{
    private readonly Enemy _enemy;

    public TargetReachedTransition(EnemyStateMachine stateMachine, Enemy enemy)
        : base(stateMachine) => _enemy = enemy;

    public override bool IsNeedTransit() => 
        _enemy.CheckAttackRange.IsAttackZone;

    public override void Transit()
    {
        base.Transit();
        StateMachine.ChangeState<AttackState>();
    }
}
