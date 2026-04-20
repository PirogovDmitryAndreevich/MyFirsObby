using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon
{
    public WeaponType Type { get; private set; }

    private Camera _camera;
    private WeaponStaticData _staticData;
    private WeaponRuntimeState _runtimeState;
    private WeaponView _weaponView;
    private IPoolService _poolService;
    private readonly IAssets _assets;

    public Weapon(WeaponStaticData staticData, WeaponProgress progress, IPoolService poolService, IAssets assets, ICoroutineRunner coroutineRunner)
    {
        _staticData = staticData;
        Type = _staticData.Type;
        _weaponView = _staticData.Prefab.GetComponent<WeaponView>();
        _poolService = poolService;
        _assets = assets;
        _runtimeState = new WeaponRuntimeState(progress, staticData, coroutineRunner);

        _poolService.RegisterFactory(() => CreateBullet());
    }

    public WeaponRuntimeState RuntimeState => _runtimeState;
    public WeaponStaticData StaticData => _staticData;
    public WeaponView WeaponView => _weaponView;

    public void OnSelected(Camera camera) =>
        _camera = camera;

    public virtual void Shoot(Vector3 shootPosition, float bulletForce, Transform muzzle)
    {
        if (!RuntimeState.CanShoot)
            return;

        Vector3 direction = CreateDirection(shootPosition, muzzle);

        int bullets = RuntimeState.TakeShot(StaticData.BulletsPerTap);

        CreateBullets(direction, bullets, bulletForce, muzzle);
    }

    protected virtual Vector3 CreateDirection(Vector3 shootPosition, Transform muzzle)
    {
        Ray ray = _camera.ScreenPointToRay(shootPosition);
        Vector3 targetPoint = ray.origin + ray.direction;

        Vector3 cameraDir = ray.direction;
        Vector3 muzzleDir = (targetPoint - muzzle.position).normalized;

        float blend = 0.9f;
        Vector3 direction = Vector3.Lerp(muzzleDir, cameraDir, blend).normalized;
        return direction;
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
            bullet.Shot(bulletDirection * bulletForce, _runtimeState.CurrentDamage);
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