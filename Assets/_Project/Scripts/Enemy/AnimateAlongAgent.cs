using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(EnemyAnimator))]
public class AnimateAlongAgent : MonoBehaviour
{
    private const float MinimalVelocity = 0.1f;

    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private EnemyAnimator _animator;

    private void Update() => 
        _animator.Move(ShouldMove());

    private bool ShouldMove() =>
        _agent.velocity.magnitude > MinimalVelocity &&
        _agent.remainingDistance > _agent.radius;
}
