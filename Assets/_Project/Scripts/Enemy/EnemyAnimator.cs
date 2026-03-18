using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour, IAnimationStateReader
{
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int IsWalk = Animator.StringToHash("IsWalk");

    private readonly int Attacking = Animator.StringToHash("Attack");
    private readonly int Walking = Animator.StringToHash("Walk");
    private readonly int Idle = Animator.StringToHash("Idle");

    private Animator _animator;

    public AnimatorState State { get; private set; }

    public event Action<AnimatorState> StateEntered;
    public event Action<AnimatorState> StateExited;

    private void Awake() =>
        _animator = GetComponent<Animator>();

    public void PlayAttack() => 
        _animator.SetTrigger(Attack);

    public void Move(bool isWalk) => 
        _animator.SetBool(IsWalk, isWalk);

    public void EnteredState(int stateHash)
    {
        State = StateFor(stateHash);
        StateEntered?.Invoke(State);
    }

    public void ExitedState(int stateHash) =>
        StateExited(StateFor(stateHash));

    private AnimatorState StateFor(int stateHash)
    {
        AnimatorState state;

        if (stateHash == Walking)
            state = AnimatorState.Walk;
        else if (stateHash == Attacking)
            state = AnimatorState.Attack;
        else if (stateHash == Idle)
            state = AnimatorState.Idle;
        else
            state = AnimatorState.Unknown;

        return state;
    }
}
