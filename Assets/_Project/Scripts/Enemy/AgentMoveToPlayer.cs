using UnityEngine;
using UnityEngine.AI;

public class AgentMoveToPlayer : Follow
{
    private const float MinimalDistance = 2f;

    [SerializeField] private NavMeshAgent _agent;

    private Transform _characterTransform;

    private void Update()
    {
        if (_characterTransform != null && IsCharacterReached())
        {
            _agent.updateRotation = true;
            _agent.destination = _characterTransform.position;
        }
    }

    public void Construct(Transform transform) => 
        _characterTransform = transform;

    private bool IsCharacterReached() =>
        Vector3.Distance(_agent.transform.position, _characterTransform.position) >= MinimalDistance;
}
