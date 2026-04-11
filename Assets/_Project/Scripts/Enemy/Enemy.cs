using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Attack _attack;
    [SerializeField] private EnemyHealth _enemyHealth;
    [SerializeField] private HealthUI _healthUI;
    [SerializeField] private AgentMoveToPlayer _agentMoveToPlayer;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private LootSpawner _lootSpawner;
    [SerializeField] private EnemyDeath _enemyDeath;

    private IPoolService _poolService;

    public Attack Attack => _attack;
    public EnemyHealth EnemyHealth => _enemyHealth;
    public HealthUI HealthUI => _healthUI;
    public AgentMoveToPlayer AgentMoveToPlayer => _agentMoveToPlayer;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public LootSpawner LootSpawner => _lootSpawner;
    public EnemyDeath EnemyDeath => _enemyDeath;

    private void OnDestroy() =>
        EnemyDeath.Happened -= OnDeath;

    public void Construct(IPoolService poolService)
    {
        _poolService = poolService;
        EnemyDeath.Happened += OnDeath;
    }

    public void ResetState() =>
        EnemyHealth.ResetState();

    private void OnDeath(EnemyDeath _) =>
        _poolService.Return(this);
}