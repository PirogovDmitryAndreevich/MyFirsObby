using UnityEngine;

public class LaserAttack : Attack
{
    [SerializeField] private Aggro _aggro;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _laserDuration = 1f;
    [SerializeField] private float _attackOffset = 0.5f;

    private Transform _character;
    private float _laserTimer;

    public override void StartAttack()
    {
        if (!CooldownIsUp())
            return;

        IsAttacking = true;

        _character = _aggro.CharacterTransform;
        _laserTimer = _laserDuration;

        _lineRenderer.enabled = true;        
    }

    public override void StopAttack()
    {
        IsAttacking = false;

        _lineRenderer.enabled = false;

        CooldownAttackTime = CooldownAttack;

        Animator.PlayAttackEnded();
    }

    public override void UpdateAttack()
    {
        if (!IsAttacking)
        {
            CooldownAttackTime -= Time.deltaTime;
            return;
        }

        if (_laserTimer > 0)
        {
            HitRay();
            Animator.PlayAttack();
            _laserTimer -= Time.deltaTime;
        }
        else
        {
            StopAttack();
        }
    }

    private void HitRay()
    {
        if (_character == null)
            return;

        Vector3 targetPoint = _character.position + Vector3.up * _attackOffset;

        Vector3 direction = (targetPoint - _firePoint.position).normalized;
        float distanceToTarget = Vector3.Distance(_firePoint.position, targetPoint);

        Ray ray = new Ray(_firePoint.position, direction);

        if (Physics.Raycast(ray, out RaycastHit hit, distanceToTarget, LayerMaskPlayer))
        {
            if (hit.collider.TryGetComponent(out IHealth health))
                health.TakeDamage(Damage * Time.deltaTime);

            DrawLaser(_firePoint.position, hit.point);
        }
        else
        {
            if (Physics.Raycast(ray, out RaycastHit obstacleHit, Mathf.Infinity))
            {
                DrawLaser(_firePoint.position, obstacleHit.point);
            }
            else
            {
                DrawLaser(_firePoint.position, _firePoint.position + direction * 100f);
            }
        }
    }

    private void DrawLaser(Vector3 start, Vector3 end)
    {
        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, end);
    }
}