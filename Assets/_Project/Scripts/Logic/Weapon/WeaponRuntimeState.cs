using System;
using System.Collections;
using UnityEngine;

public class WeaponRuntimeState
{
    private WeaponProgress _weapon;
    private WeaponStaticData _staticData;
    private readonly ICoroutineRunner _coroutineRunner;

    private float _reloadingProgress;
    private int _currentAmmo;
    private float _nextShotTime;

    public event Action AmmunitionChanged;
    public event Action<float> ReloadProgressChanged;
    public event Action ReloadStarted;
    public event Action ReloadFinished;

    public WeaponRuntimeState(WeaponProgress weapon, WeaponStaticData staticDataService, ICoroutineRunner coroutineRunner)
    {
        _weapon = weapon;
        _staticData = staticDataService;
        _coroutineRunner = coroutineRunner;

        OnUpdateLevel();
        OnUpdateMagazineSize();

        CurrentAmmo = CurrentMagazineSize;

        Subscribe();
    }

    public int CurrentDamage { get; private set; }
    public int CurrentMagazineSize { get; private set; }
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

    public void Unsubscribe() =>
        _weapon.WeaponsUpdate -= OnUpdateWeapon;

    private void Subscribe() =>
        _weapon.WeaponsUpdate += OnUpdateWeapon;

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
        CurrentAmmo = CurrentMagazineSize;

        IsReloading = false;
        ReloadFinished?.Invoke();
    }

    private void OnUpdateWeapon(WeaponsUpdateType updateType)
    {
        switch (updateType)
        {
            case WeaponsUpdateType.MagazineSize:
                OnUpdateMagazineSize();
                break;
            case WeaponsUpdateType.Damage:
                OnUpdateLevel();
                break;
        }
    }

    private void OnUpdateLevel() =>
        CurrentDamage = _staticData.GetLevelInfo(_weapon.DamageLevel, WeaponsUpdateType.Damage).Value;

    private void OnUpdateMagazineSize()
    {
        CurrentMagazineSize = _staticData.GetLevelInfo(_weapon.MagazineLevel, WeaponsUpdateType.MagazineSize).Value;
        CurrentAmmo = CurrentMagazineSize;
    }
}