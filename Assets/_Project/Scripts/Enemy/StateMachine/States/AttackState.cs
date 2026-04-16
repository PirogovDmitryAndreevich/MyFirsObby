public class AttackState : EnemyState
{
    public AttackState(EnemyStateMachine stateMachine, Enemy enemy)
        : base(stateMachine, enemy)
    {
        Transitions = new Transition[]
        {
            new SeeTargetTransition(stateMachine,enemy),
            new LostTargetTransition(stateMachine,enemy)
        };
    }

    public override void Enter()
    {
        Enemy.AgentMoveToTarget.StopMove();
        Enemy.EnemyAnimator.Move(false);
    }

    public override void Update()
    {
        Enemy.Attack.UpdateCooldown();

        if (!Enemy.Attack.IsAttacking && Enemy.CheckAttackRange.IsAttackZone)
            Enemy.Attack.StartAttack();
    }

    public override void TryTransit()
    {
        if (!Enemy.CheckAttackRange.IsAttackZone)
            base.TryTransit();
    }
}