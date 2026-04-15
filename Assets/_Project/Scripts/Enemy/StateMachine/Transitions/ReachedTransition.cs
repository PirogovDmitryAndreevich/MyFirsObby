using UnityEngine;

public class ReachedTransition : Transition
{
    private readonly Enemy _enemy;

    public ReachedTransition(EnemyStateMachine stateMachine, Enemy enemy) : base(stateMachine) =>
        _enemy = enemy;

    public override bool IsNeedTransit()
    {
        if (_enemy.CurrentTarget != null)
            return Vector3.Distance(_enemy.transform.position, _enemy.CurrentTarget.position) <= _enemy.MinimalDistance;
        else
            return false;
    }    
}
