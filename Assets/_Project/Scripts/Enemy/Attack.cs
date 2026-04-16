using System.Linq;
using UnityEngine;

[RequireComponent(typeof(EnemyAnimationEvents), typeof(EnemyAnimator))]
public class Attack : MonoBehaviour
{
    [SerializeField] private EnemyAnimator _animator;
    [SerializeField] private EnemyAnimationEvents _animationEvents;
    [SerializeField] private LayerMask _layerMaskPlayer;

    private float _cooldownAttack = 3f;
    private float _cleavage = 1f;
    private float _effectiveDistance = 0.5f;
    private float _damage = 10f;

    private float _cooldownAttackTime;
    private Collider[] _hits = new Collider[1];

    public bool IsAttacking { get; private set; }

    private void OnEnable()
    {
        _animationEvents.Attack += OnAttack;
        _animationEvents.AttackEnded += OnAttackEnded;
    }

    private void OnDisable()
    {
        _animationEvents.Attack -= OnAttack;
        _animationEvents.AttackEnded -= OnAttackEnded;
    }

    public void Construct(float damage, float effectiveDistance, float cleavage, float cooldownAttack)
    {
        _damage = damage;
        _effectiveDistance = effectiveDistance;
        _cleavage = cleavage;
        _cooldownAttack = cooldownAttack;
    }

    public void StartAttack()
    {
        IsAttacking = true;
        _animator.PlayAttack();
    }

    public void UpdateCooldown()
    {
        if (!CooldownIsUp())
            _cooldownAttackTime -= Time.deltaTime;            
    }

    private void OnAttack()
    {
        if (Hit(out Collider hit))
        {
            PhysicsDebug.DrawDebug(StartPoint(), _cleavage, 1f);

            if (hit != null && hit.TryGetComponent(out IHealth characterHealth))
                characterHealth.TakeDamage(_damage);
        }
    }

    private void OnAttackEnded()
    {
        IsAttacking = false;
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

    private bool CooldownIsUp() =>
        _cooldownAttackTime <= 0;
}
