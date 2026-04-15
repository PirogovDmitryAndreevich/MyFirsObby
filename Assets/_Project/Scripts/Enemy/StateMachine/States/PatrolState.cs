public class PatrolState : EnemyState
{
    public PatrolState(EnemyStateMachine stateMachine, Enemy enemy) : base(stateMachine, enemy)
    {
        Transitions = new Transition[]
            {
                new SeeTargetTransition(stateMachine, enemy),
                new WayPointReachedTransition(stateMachine, enemy)
            };

        ChangeTarget();
    }

    public override void Enter()
    {
        ChangeTarget();
        Enemy.EnemyAnimator.Move(true);
    }

    public override void Update() =>
        Enemy.AgentMoveToTarget.Move(Enemy.CurrentTarget);

    private void ChangeTarget() => 
        Enemy.ChangeWaypoint();
}
