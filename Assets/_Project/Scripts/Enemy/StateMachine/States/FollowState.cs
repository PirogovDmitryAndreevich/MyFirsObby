public class FollowState : EnemyState
{
    public FollowState(EnemyStateMachine stateMachine, Enemy enemy) : base(stateMachine, enemy)
    {
        Transitions = new Transition[]
        {
            new LostTargetTransition(stateMachine, enemy),
            new TargetReachedTransition(stateMachine, enemy)
        };
    }

    public override void Enter()
    {
        Enemy.SetTarget(Enemy.Aggro.CharacterTransform);
        Enemy.EnemyAnimator.Move(true);
    }

    public override void Update()
    {
        if (Enemy.Aggro.CharacterTransform != null)
            Enemy.AgentMoveToTarget.Move(Enemy.CurrentTarget);
    }
}
