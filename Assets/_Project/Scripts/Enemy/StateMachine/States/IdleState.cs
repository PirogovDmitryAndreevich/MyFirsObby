using UnityEngine;

public class IdleState : EnemyState
{
    private float _endWaitTime;

    public IdleState(EnemyStateMachine stateMachine, Enemy enemy) : base(stateMachine, enemy)
    {
        Transitions = new Transition[]
        {
            new EndIdleTransition(stateMachine,this),
            new SeeTargetTransition(stateMachine, enemy)
        };
    }

    public bool isEndWait =>
        _endWaitTime <= Time.time;

    public override void Enter()
    {
        _endWaitTime = Time.time + Enemy.WaitTime;
        Enemy.EnemyAnimator.Move(false);
    }
}
