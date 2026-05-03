using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon
{
    public WeaponType Type { get; private set; }

    private WeaponStaticData _staticData;
    private WeaponRuntimeState _runtimeState;
    private IPoolService _poolService;
    private readonly IAssets _assets;

    public Weapon(WeaponStaticData staticData, IPoolService poolService, IAssets assets, ICoroutineRunner coroutineRunner)
    {
        _staticData = staticData;
        Type = _staticData.Type;
        _poolService = poolService;
        _assets = assets;
        _runtimeState = new WeaponRuntimeState( staticData, coroutineRunner);

        _poolService.RegisterFactory(() => CreateBullet());
    }

    public WeaponRuntimeState RuntimeState => _runtimeState;
    public WeaponStaticData StaticData => _staticData;

    public virtual void Shoot(Vector3 aimPoint, float bulletForce, Transform muzzle)
    {
        if (!RuntimeState.CanShoot)
            return;

        int bullets = RuntimeState.TakeShot(StaticData.BulletsPerTap);

        Vector3 direction = (aimPoint - muzzle.position).normalized;

        CreateBullets(direction, bullets, bulletForce, muzzle);
    }

    protected virtual Bullet CreateBullet()
    {
        var bullet = _assets.Instantiate(StaticData.BulletPrefab);
        bullet.Construct(_poolService);
        return bullet;
    }

    private void CreateBullets(Vector3 direction, int bulletsCount, float bulletForce, Transform muzzle)
    {
        for (int i = 0; i < bulletsCount; i++)
        {
            Vector3 bulletDirection = DirectionWithSpread(direction);

            Bullet bullet = _poolService.Get<Bullet>();
            bullet.transform.position = muzzle.position;
            bullet.Shot(bulletDirection * bulletForce, _runtimeState.Damage);
        }
    }

    private Vector3 DirectionWithSpread(Vector3 direction)
    {
        Vector2 spread = Random.insideUnitCircle * StaticData.Spread;
        Quaternion spreadRot = Quaternion.Euler(spread.x, spread.y, 0);
        direction = spreadRot * direction;
        return direction;
    }
}