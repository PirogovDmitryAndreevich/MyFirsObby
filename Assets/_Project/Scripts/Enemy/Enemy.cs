using System.Collections;
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
    private MonsterStaticData _staticData;
    private IPoolService _poolService;
    private IWaypointChangerService _waypointChanger;
    private IPersistentProgressService _progress;
    private ISaveLoadService _save;
    private bool _isFrozen;

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

    private void OnEnable() => 
        EnemyDeath.Happened += OnDeath;

    private void OnDisable() => 
        EnemyDeath.Happened -= OnDeath;


    private void Start() =>
        _stateMachine = new EnemyStateMachine(this);

    private void Update() =>
        _stateMachine.Update();

    public void Construct(MonsterStaticData staticData, IPoolService poolService, IWaypointChangerService waypointChanger, IPersistentProgressService progress, ISaveLoadService save)
    {
        _staticData = staticData;
        _progress = progress;
        _save = save;
        _poolService = poolService;
        _waypointChanger = waypointChanger;
    }

    public void ResetState()
    {
        int multiplier = _progress.Progress.PlayerState.Multiplier;

        EnemyHealth.Max = _staticData.Health * multiplier;
        LootSpawner.SetLoot(_staticData.MinLoot * multiplier, _staticData.MaxLoot * multiplier);
        EnemyHealth.ResetState();
    }

    public void SetTarget(Transform target) =>
        CurrentTarget = target;

    public void Freeze(float time)
    {
        if (_isFrozen)
            return;

        StartCoroutine(Freezing(time));
    }

    public void ChangeWaypoint() =>
        CurrentTarget = _waypointChanger.GetNextPoint().transform;

    private void OnDeath(EnemyDeath _)
    {
        AddScore();
        _poolService.Return(this);
    }

    private void AddScore()
    {
        _progress.Progress.PlayerState.AddScore(_staticData.RewardScores * _progress.Progress.PlayerState.Multiplier);
        _save.SaveProgress();
    }

    private IEnumerator Freezing(float time)
    {
        _isFrozen = true;
        float freezing = time + Time.time;

        Attack.enabled = false;
        EnemyAnimator.enabled = false;

        while (freezing > Time.time)
        {
            AgentMoveToTarget.StopMove();
            yield return null;
        }
        
        _isFrozen = false;
        EnemyAnimator.enabled = true;
        Attack.enabled = true;
    }
}