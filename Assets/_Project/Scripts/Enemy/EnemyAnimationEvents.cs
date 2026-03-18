using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimationEvents : MonoBehaviour
{
    public event Action Attack;
    public event Action AttackEnded;

    public void AttackAnimation() =>
        Attack?.Invoke();

    public void AttackEndedAnimation() =>
        AttackEnded?.Invoke();
}
