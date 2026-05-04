using System;
using System.Collections;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] private Transform _muzzle;
    [SerializeField] private Transform _view;

    public WeaponType Type { get; private set; }

    private WeaponStaticData _staticData;
    private IBulletsPool _poolService;

    private float _reloadingProgress;
    private int _currentAmmo;
    private float _nextShotTime;

    public event Action AmmunitionChanged;
    public event Action<float> ReloadProgressChanged;
    public event Action ReloadStarted;
    public event Action ReloadFinished;

    public Transform Muzzle => _muzzle;

    public bool CanShoot => Time.time >= _nextShotTime && !IsReloading && CurrentAmmo > 0;

    public bool IsReloading { get; private set; }

    public float Distance => _staticData.Distance;

    public int MagazineSize => _staticData.MagazineSize;

    public int CurrentAmmo
    {
        get => _currentAmmo;
        private set
        {
            if (_currentAmmo != value)
            {
                _currentAmmo = value;
                AmmunitionChanged?.Invoke();
            }
        }
    }

    public float ReloadProgress
    {
        get => _reloadingProgress;
        private set
        {
            if (_reloadingProgress != value)
            {
                _reloadingProgress = value;
                ReloadProgressChanged?.Invoke(_reloadingProgress);
            }
        }
    }

    public void Construct(WeaponStaticData staticData, IBulletsPool poolService)
    {
        _staticData = staticData;
        Type = _staticData.Type;
        _poolService = poolService;

        CurrentAmmo = _staticData.MagazineSize;
    }

    public virtual void Shoot(Vector3 direction)
    {
        if (!CanShoot)
            return;       

        CreateBullets(direction);
    }

    public void SetView(bool isView) =>
        _view.gameObject.SetActive(isView);

    protected void CreateBullets(Vector3 direction)
    {
        int bulletsCount = TakeShot();

        for (int i = 0; i < bulletsCount; i++)
        {
            Vector3 bulletDirection = DirectionWithSpread(direction);

            Bullet bullet = _poolService.Get(_staticData.BulletType);
            bullet.transform.position = Muzzle.position;
            bullet.Shot(bulletDirection, _staticData.Damage);
        }
    }

    private Vector3 DirectionWithSpread(Vector3 direction)
    {
        Vector2 spread = UnityEngine.Random.insideUnitCircle * _staticData.Spread;
        Quaternion spreadRot = Quaternion.Euler(spread.x, spread.y, 0);
        direction = spreadRot * direction;
        return direction;
    }

    private int TakeShot()
    {
        if (!CanShoot)
            return 0;

        int bullets = Mathf.Min(_staticData.BulletsPerTap, CurrentAmmo);

        CurrentAmmo -= bullets;
        _nextShotTime = Time.time + _staticData.TimeBetweenShot;

        if (CurrentAmmo <= 0)
            StartReload();

        return bullets;
    }

    private void StartReload()
    {
        if (IsReloading)
            return;

        IsReloading = true;
        StartCoroutine(ReloadCoroutine());

        ReloadStarted?.Invoke();
    }

    private IEnumerator ReloadCoroutine()
    {
        float duration = _staticData.ReloadTime;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            ReloadProgress = Mathf.Clamp01(elapsed / duration);

            yield return null;
        }

        FinishReload();
    }

    private void FinishReload()
    {
        CurrentAmmo = _staticData.MagazineSize;

        IsReloading = false;
        ReloadFinished?.Invoke();
    }
}