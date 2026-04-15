public class SeeTargetTransition : Transition
{
    private readonly Enemy _enemy;

    public SeeTargetTransition(EnemyStateMachine stateMachine, Enemy enemy) : base(stateMachine) => 
        _enemy = enemy;

    public override bool IsNeedTransit() =>
        _enemy.Aggro.HasAggroTarget;

    public override void Transit()
    {
        base.Transit();
        StateMachine.ChangeState<FollowState>();
    }
}
