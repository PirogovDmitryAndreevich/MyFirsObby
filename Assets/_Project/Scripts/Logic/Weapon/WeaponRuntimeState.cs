using System;
using System.Collections;
using UnityEngine;

public class WeaponRuntimeState
{
    private WeaponStaticData _staticData;
    private readonly ICoroutineRunner _coroutineRunner;

    private float _reloadingProgress;
    private int _currentAmmo;
    private float _nextShotTime;

    public event Action AmmunitionChanged;
    public event Action<float> ReloadProgressChanged;
    public event Action ReloadStarted;
    public event Action ReloadFinished;

    public WeaponRuntimeState(WeaponStaticData staticDataService, ICoroutineRunner coroutineRunner)
    {
        _staticData = staticDataService;
        _coroutineRunner = coroutineRunner;

        Damage = _staticData.Damage;
        MagazineSize = _staticData.MagazineSize;

        CurrentAmmo = MagazineSize;
    }

    public int Damage { get; private set; }
    public int MagazineSize { get; private set; }
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
    public bool CanShoot => Time.time >= _nextShotTime && !IsReloading && CurrentAmmo > 0;
    public bool IsReloading { get; private set; }
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

    public int TakeShot(int bulletsToShot)
    {
        if (!CanShoot)
            return 0;

        int bullets = Mathf.Min(bulletsToShot, CurrentAmmo);

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
        _coroutineRunner.StartCoroutine(ReloadCoroutine());

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
        CurrentAmmo = MagazineSize;

        IsReloading = false;
        ReloadFinished?.Invoke();
    }
}