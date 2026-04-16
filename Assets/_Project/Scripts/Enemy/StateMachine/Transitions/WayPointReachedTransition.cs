using UnityEngine;

public class WayPointReachedTransition : Transition
{
    private readonly Enemy _enemy;

    public WayPointReachedTransition(EnemyStateMachine stateMachine, Enemy enemy)
        : base(stateMachine) => _enemy = enemy;

    public override bool IsNeedTransit()
    {
        if (_enemy.CurrentTarget != null)
            return Vector3.Distance(_enemy.transform.position, _enemy.CurrentTarget.position) <= _enemy.MinimalDistance;
        else
            return false;
    }

    public override void Transit()
    {
        base.Transit();
        StateMachine.ChangeState<IdleState>();
    }
}
