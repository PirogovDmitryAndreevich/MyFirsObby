using UnityEngine;
using UnityEngine.AI;

public class AgentMoveToPlayer : Follow
{
    private const float MinimalDistance = 2f;

    [SerializeField] private NavMeshAgent _agent;

    private Transform _characterTransform;
    private IGameFactory _gameFactory;

    private void Start()
    {
        _gameFactory = AllServices.Container.Single<IGameFactory>();

        if (_gameFactory.CharacterObject != null)
            SetCharacterTransform();
        else
            _gameFactory.CharacterCreated += SetCharacterTransform;
    }    

    private void Update()
    {
        if (_characterTransform != null && IsCharacterReached())
        {
            _agent.updateRotation = true;
            _agent.destination = _characterTransform.position;
        }
    }

    private void SetCharacterTransform()
    {
        _gameFactory.CharacterCreated -= SetCharacterTransform;
        _characterTransform = _gameFactory.CharacterObject.transform;
    }

    private bool IsCharacterReached() =>
        Vector3.Distance(_agent.transform.position, _characterTransform.position) >= MinimalDistance;
}
