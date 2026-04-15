using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    private EnemyState CurrentState;
    private Dictionary<Type, EnemyState> States;

    public EnemyStateMachine(Enemy enemy)
    {
        States = new Dictionary<Type, EnemyState>()
        {
            {typeof(IdleState), new IdleState(this,enemy)},
            {typeof(PatrolState), new PatrolState(this,enemy)},
            {typeof(FollowState), new FollowState(this,enemy)},
            {typeof(AttackState), new AttackState(this,enemy)}
        };

        ChangeState<PatrolState>();
    }

    public void Update()
    {
        if (CurrentState == null)
            return;

        CurrentState.Update();

        CurrentState.TryTransit();
    }

    public void ChangeState<TState>() where TState : EnemyState
    {
        if (CurrentState != null && CurrentState.GetType() == typeof(TState))
            return;

        if (States.TryGetValue(typeof(TState), out EnemyState newState))
        {
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }

        Debug.Log($"{typeof(TState)}");
    }
}
