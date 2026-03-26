using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shooter : MonoBehaviour
{
    [Header("Bullet setting")]
    [SerializeField] private float _bulletForce = 500f;
    [SerializeField] private float _shootDistance = 200f;

    private Camera _camera;
    private WeaponStaticData _weaponData;
    private Transform _muzzle;

    private int _damage;    
    private int _magazineSize;

    private bool CanShot = true;
    private int Ammunition;

    public event Action AmmunitionChanged;

    public WeaponStaticData CurrentWeapon => _weaponData;

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
    public int CurrentMagazineSize => _magazineSize;

    private void Start()
    {
        _camera = Camera.main;
    }

    public void SelectWeapon(WeaponStaticData weaponData, Transform muzzle ,int damageLevel, int magazineLevel)
    {
        _weaponData = weaponData;
        _damage = weaponData.GetDamage(damageLevel);
        _magazineSize = weaponData.GetMagazineLength(magazineLevel);
        _muzzle = muzzle;
    }

    public void Shoot(Vector3 shootPosition)
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

        Vector3 direction = (targetPoint - _muzzle.position).normalized;


        Vector2 spread = Random.insideUnitCircle * _weaponData.Spread;

        direction += new Vector3(spread.x, spread.y, 0);
        direction.Normalize();

        var bullet = Instantiate(_weaponData.BulletPrefab, _muzzle.position, Quaternion.LookRotation(direction));
        bullet.Shot(direction * _bulletForce, _damage);
    }

    protected virtual void Reload()
    {
        CanShot = false;
        CurrentAmmunition = _magazineSize;
        CanShot = true;
    }
}
