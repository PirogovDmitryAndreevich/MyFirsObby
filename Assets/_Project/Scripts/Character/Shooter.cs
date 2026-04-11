using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shooter : MonoBehaviour
{
    [Header("Bullet setting")]
    [SerializeField] private float _bulletForce = 500f;
    [SerializeField] private float _shootDistance = 200f;
    [SerializeField] private float _fireRate = 0.15f;

    private Camera _camera;
    private WeaponStaticData _weaponData;
    private Transform _muzzle;

    private int _damage;
    private int _magazineSize;

    private bool _canShoot = true;
    private float _nextShootTime;

    private int Ammunition;
    private IPoolService _poolService;

    public event Action AmmunitionChanged;

    public bool CanShootNow => _canShoot && Time.time >= _nextShootTime;
    public WeaponStaticData CurrentWeapon => _weaponData;

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

    private void Start() => 
        _camera = Camera.main;

    public void Construct(IPoolService poolService, IGameFactory gameFactory)
    {
        _poolService = poolService;
        _poolService.RegisterFactory(() => gameFactory.CreateBullet());
    }

    public void SelectWeapon(WeaponStaticData weaponData, Transform muzzle, int damageLevel, int magazineLevel)
    {
        _weaponData = weaponData;

        PriceWeaponLevels levelStatsDamage = _weaponData.GeLevelInfo(damageLevel, WeaponsUpdateType.Damage);
        PriceWeaponLevels levelStatsMagazine = _weaponData.GeLevelInfo(magazineLevel, WeaponsUpdateType.MagazineSize);

        if (levelStatsDamage != null)
            _damage = levelStatsDamage.Value;

        if (levelStatsMagazine != null)
            _magazineSize = levelStatsMagazine.Value;

        _muzzle = muzzle;

        CurrentAmmunition = _magazineSize;
    }

    public void TryShoot(Vector3 shootPosition)
    {
        if (!CanShootNow)
            return;

        Shoot(shootPosition);
        _nextShootTime = Time.time + _fireRate;
    }

    private void Shoot(Vector3 shootPosition)
    {
        if (CurrentAmmunition <= 0)
        {
            Reload();
            return;
        }

        CurrentAmmunition--;


        Ray ray = _camera.ScreenPointToRay(shootPosition);
        Vector3 targetPoint = ray.origin + ray.direction;

        Vector3 cameraDir = ray.direction;
        Vector3 muzzleDir = (targetPoint - _muzzle.position).normalized;

        float blend = 0.9f;
        Vector3 direction = Vector3.Lerp(muzzleDir, cameraDir, blend).normalized;

        Vector2 spread = Random.insideUnitCircle * _weaponData.Spread;
        Quaternion spreadRot = Quaternion.Euler(spread.x, spread.y, 0);
        direction = spreadRot * direction;

        Bullet bullet = _poolService.Get<Bullet>();
        bullet.transform.position = _muzzle.position;
        bullet.Shot(direction * _bulletForce, _damage);
    }

    private void Reload()
    {
        _canShoot = false;
        CurrentAmmunition = _magazineSize;
        _canShoot = true;
    }
}
