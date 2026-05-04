using System.Collections;
using UnityEngine;

public class LaserGun : WeaponBase
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _freezeTime;
    [SerializeField] private float _laserDuration;

    private bool _isFiring;

    private void Start()
    {
        _lineRenderer.enabled = false;
        _isFiring = false;
    }

    public override void Shoot(Vector3 direction)
    {
        if (!CanShoot || _isFiring)
            return;

        TakeShot();
        StartCoroutine(StartLaser(direction));
    }

    private IEnumerator StartLaser(Vector3 direction)
    {
        _isFiring = true;

        float duration = _laserDuration + Time.time;
        _lineRenderer.enabled = true;

        while (duration > Time.time)
        {
            Vector3 start = Muzzle.position;
            Vector3 end = start + direction * Distance; ;

            RaycastHit[] hits = Physics.RaycastAll(start, direction, Distance);

            foreach (var hit in hits)
            {
                if (hit.collider.TryGetComponent(out Enemy enemy))
                {
                    enemy.Freeze(_freezeTime);
                }
            }

            _lineRenderer.SetPosition(0, start);
            _lineRenderer.SetPosition(1, end);

            yield return null;
        }

        _lineRenderer.enabled = false;
        _isFiring = false;

        StartReload();
    }
}