using System;
using System.Collections;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] private Transform _muzzle;
    [SerializeField] private Transform _view;
    [SerializeField] private AudioSource _audioSource;

    public WeaponType Type { get; private set; }

    protected WeaponStaticData StaticData;
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

    public float Distance => StaticData.Distance;

    public int MagazineSize => StaticData.MagazineSize;

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
        StaticData = staticData;
        Type = StaticData.Type;
        _poolService = poolService;

        CurrentAmmo = StaticData.MagazineSize;
    }

    public virtual void Shoot(Vector3 direction)
    {
        if (!CanShoot)
            return;

        _audioSource.PlayOneShot(StaticData.ShootSound);
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

            Bullet bullet = _poolService.Get(StaticData.BulletType);
            bullet.transform.position = Muzzle.position;
            bullet.Shot(bulletDirection, StaticData.Damage);
        }
    }

    protected void StartReload()
    {
        if (IsReloading)
            return;

        IsReloading = true;
        StartCoroutine(ReloadCoroutine());

        ReloadStarted?.Invoke();
    }

    protected int TakeShot()
    {
        if (!CanShoot)
            return 0;

        int bullets = Mathf.Min(StaticData.BulletsPerTap, CurrentAmmo);

        CurrentAmmo -= bullets;
        _nextShotTime = Time.time + StaticData.TimeBetweenShot;

        if (CurrentAmmo <= 0)
            StartReload();

        return bullets;
    }

    private Vector3 DirectionWithSpread(Vector3 direction)
    {
        Vector2 spread = UnityEngine.Random.insideUnitCircle * StaticData.Spread;
        Quaternion spreadRot = Quaternion.Euler(spread.x, spread.y, 0);
        direction = spreadRot * direction;
        return direction;
    }        

    private IEnumerator ReloadCoroutine()
    {
        float duration = StaticData.ReloadTime;
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
        CurrentAmmo = StaticData.MagazineSize;

        IsReloading = false;
        ReloadFinished?.Invoke();
    }
}