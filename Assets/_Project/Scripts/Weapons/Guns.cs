using System;
using UnityEngine;

public class Guns : MonoBehaviour
{
    [Header("Gun setting")]
    [SerializeField] protected Transform Muzzle;

    [Header("Bullet setting")]
    [SerializeField] protected Bullet Bullet;
    [SerializeField] protected float BulletForce = 500f;

    [Header("Gun stats")]
    [SerializeField] protected int MagazineSize;
    [SerializeField] protected int Damage = 5;
    [SerializeField] protected float SpeedShot = 5f;
    [SerializeField] protected float Spread = 0f;
    [SerializeField] private float _shootDistance = 200f;

    protected bool CanShot = true;
    protected int Ammunition;

    private Camera _camera;

    public event Action AmmunitionChanged;

    public bool CanShotNow => CanShot;
    public int CurrentAmmunition
    {
        get => Ammunition;
        set
        {
            if (Ammunition != value)
            {
                Ammunition = value;
                AmmunitionChanged?.Invoke();
            }
        }
    }
    public int CurrentMagazineSize => MagazineSize;

    private void Awake()
    {
        _camera = Camera.main;
        CurrentAmmunition = MagazineSize;
    }

    public virtual void Shoot(Vector3 shootPosition)
    {
        if (CurrentAmmunition <= 0)
        {
            Reload();
            return;
        }

        CurrentAmmunition--;

        Ray ray = _camera.ScreenPointToRay(shootPosition);

        RaycastHit hit;
        Vector3 targetPoint =
            Physics.Raycast(ray, out hit, _shootDistance)
            ? hit.point
            : ray.GetPoint(_shootDistance);

        Vector3 direction = (targetPoint - Muzzle.position).normalized;


        Vector2 spread = UnityEngine.Random.insideUnitCircle * Spread;

        direction += new Vector3(spread.x, spread.y, 0);
        direction.Normalize();

        var bullet = Instantiate(Bullet, Muzzle.position, Quaternion.LookRotation(direction));
        bullet.Shot(direction * BulletForce, Damage);
    }

    protected virtual void Reload()
    {
        CanShot = false;
        CurrentAmmunition = MagazineSize;
        CanShot = true;
    }
}
