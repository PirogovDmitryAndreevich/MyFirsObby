using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _waitTime;
    [SerializeField] private float _minimalDistance;
    [SerializeField] private Attack _attack;
    [SerializeField] private CheckAttackRange _attackRange;
    [SerializeField] private EnemyHealth _enemyHealth;
    [SerializeField] private HealthUI _healthUI;
    [SerializeField] private AgentMoveToTarget _agentMoveToTarget;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private LootSpawner _lootSpawner;
    [SerializeField] private EnemyDeath _enemyDeath;
    [SerializeField] private EnemyAnimator _enemyAnimator;
    [SerializeField] private Aggro _aggro;

    private EnemyStateMachine _stateMachine;
    private IPoolService _poolService;
    private IWaypointChangerService _waypointChanger;

    public Attack Attack => _attack;
    public EnemyHealth EnemyHealth => _enemyHealth;
    public HealthUI HealthUI => _healthUI;
    public AgentMoveToTarget AgentMoveToTarget => _agentMoveToTarget;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public LootSpawner LootSpawner => _lootSpawner;
    public EnemyDeath EnemyDeath => _enemyDeath;
    public EnemyAnimator EnemyAnimator => _enemyAnimator;
    public Aggro Aggro => _aggro;
    public CheckAttackRange CheckAttackRange => _attackRange;
    public Transform CurrentTarget { get; private set; }
    public float WaitTime => _waitTime;
    public float MinimalDistance => _minimalDistance;

    private void OnEnable()
    {
        EnemyDeath.Happened += OnDeath;
        ResetState();
    }

    private void OnDisable()
    {
        EnemyDeath.Happened -= OnDeath;
    }


    private void Start() => 
        _stateMachine = new EnemyStateMachine(this);

    private void Update() => 
        _stateMachine.Update();

    public void Construct(IPoolService poolService, IWaypointChangerService waypointChanger)
    {
        _poolService = poolService;
        _waypointChanger = waypointChanger;
    }

    public void ResetState() =>
        EnemyHealth.ResetState();

    public void SetTarget(Transform target) => 
        CurrentTarget = target;

    public void ChangeWaypoint() => 
        CurrentTarget = _waypointChanger.GetNextPoint().transform;

    private void OnDeath(EnemyDeath _) =>
        _poolService.Return(this);
}