using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentMoveToTarget : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;

    public void Move(Transform target)
    {
        if (target != null)
        {
            _agent.isStopped = false;
            _agent.updateRotation = true;
            _agent.destination = target.position;
        }
    }

    public void StopMove()
    {
        _agent.isStopped = true;
        _agent.ResetPath();
        _agent.velocity = Vector3.zero;
    }
}
