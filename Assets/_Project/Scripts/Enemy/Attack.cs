using System.Linq;
using UnityEngine;

[RequireComponent(typeof(EnemyAnimationEvents), typeof(EnemyAnimator))]
public class Attack : MonoBehaviour
{
    [SerializeField] protected LayerMask LayerMaskPlayer;
    [SerializeField] protected EnemyAnimator Animator;
    [SerializeField] private EnemyAnimationEvents _animationEvents;

    protected float Damage = 10f;
    protected float CooldownAttackTime;
    protected float CooldownAttack = 3f;
    private float _cleavage = 1f;
    private float _effectiveDistance = 0.5f;

    private Collider[] _hits = new Collider[1];

    public bool IsAttacking { get; protected set; }

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
        Damage = damage;
        _effectiveDistance = effectiveDistance;
        _cleavage = cleavage;
        CooldownAttack = cooldownAttack;
    }

    public virtual void StartAttack()
    {
        IsAttacking = true;
        Animator.PlayAttack();
    }

    public virtual void UpdateAttack()
    {
        if (!CooldownIsUp())
            CooldownAttackTime -= Time.deltaTime;            
    }

    public virtual void StopAttack()
    {
        OnAttackEnded();
    }

    protected virtual void OnAttack()
    {
        if (Hit(out Collider hit))
        {
            PhysicsDebug.DrawDebug(StartPoint(), _cleavage, 1f);

            if (hit != null && hit.TryGetComponent(out IHealth characterHealth))
                characterHealth.TakeDamage(Damage);
        }
    }

    protected virtual void OnAttackEnded()
    {
        IsAttacking = false;
        CooldownAttackTime = CooldownAttack;
    }

    protected bool CooldownIsUp() =>
        CooldownAttackTime <= 0;

    private bool Hit(out Collider hit)
    {
        int hitCount = Physics.OverlapSphereNonAlloc(StartPoint(), _cleavage, _hits, LayerMaskPlayer);

        hit = _hits.FirstOrDefault();

        return hitCount > 0;
    }

    private Vector3 StartPoint() =>
        new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z)
        + transform.forward * _effectiveDistance;
}
