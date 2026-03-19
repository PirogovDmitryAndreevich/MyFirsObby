using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(EnemyAnimationEvents), typeof(EnemyAnimator))]
public class Attack : MonoBehaviour
{
    [SerializeField] private EnemyAnimator _animator;
    [SerializeField] private EnemyAnimationEvents _animationEvents;
    [SerializeField] private float _cooldownAttack = 3f;
    [SerializeField] private LayerMask _layerMaskPlayer;
    [SerializeField] private float _cleavage = 1f;
    [SerializeField] private float _effectiveDistance = 0.5f;
    [SerializeField] private float _damage = 10f;

    private IGameFactory _gameFactory;
    private Transform _characterTransform;
    private float _cooldownAttackTime;
    private bool _isAttacking;
    private Collider[] _hits = new Collider[1];
    private bool _attackIsActive;

    private void Awake()
    {
        _gameFactory = AllServices.Container.Single<IGameFactory>();

        if (_gameFactory.CharacterObject != null)
            SetCharacterTransform();
        else
            _gameFactory.CharacterCreated += CharacterCreated;
    }

    private void Start()
    {
        _animationEvents.Attack += OnAttack;
        _animationEvents.AttackEnded += OnAttackEnded;
    }

    private void OnDestroy()
    {
        _animationEvents.Attack -= OnAttack;
        _animationEvents.AttackEnded -= OnAttackEnded;
    }

    private void Update()
    {
        UpdateCooldown();

        if (CanAttack())
            StartAttack();
    }

    public void DisableAttack() => 
        _attackIsActive = false;

    public void EnableAttack() => 
        _attackIsActive = true;

    private void StartAttack()
    {
        _isAttacking = true;

        //transform.LookAt(_characterTransform);
        _animator.PlayAttack();
    }

    private void OnAttack()
    {
        if (Hit(out Collider hit))
        {
            PhysicsDebug.DrawDebug(StartPoint(), _cleavage, 1f);
            if (hit != null && hit.TryGetComponent(out CharacterHealth characterHealth))
                characterHealth.TakeDame(_damage);

        }
    }

    private void OnAttackEnded()
    {
        _isAttacking = false;
        _cooldownAttackTime = _cooldownAttack;
    }

    private bool Hit(out Collider hit)
    {
        int hitCount = Physics.OverlapSphereNonAlloc(StartPoint(), _cleavage, _hits, _layerMaskPlayer);

        hit = _hits.FirstOrDefault();

        return hitCount > 0;
    }

    private Vector3 StartPoint() =>
        new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z)
        + transform.forward * _effectiveDistance;

    private void UpdateCooldown()
    {
        if (!CooldownIsUp())
            _cooldownAttackTime -= Time.deltaTime;
    }

    private bool CanAttack() =>
        _attackIsActive && !_isAttacking && CooldownIsUp();

    private bool CooldownIsUp() =>
        _cooldownAttackTime <= 0;

    private void SetCharacterTransform() =>
        _characterTransform = _gameFactory.CharacterObject.transform;

    private void CharacterCreated()
    {
        _gameFactory.CharacterCreated -= CharacterCreated;
        SetCharacterTransform();
    }    
}
