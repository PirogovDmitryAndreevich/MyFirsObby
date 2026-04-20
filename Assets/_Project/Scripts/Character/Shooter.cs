using System;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Bullet setting")]
    [SerializeField] private float _bulletForce = 500f;

    private Camera _camera;
    private Transform _muzzle;
    private bool _isWeaponSelected;

    public event Action StateChanged;

    public Weapon CurrentWeapon { get; private set; }
    public bool IsWeaponSelected
    {
        get => _isWeaponSelected;
        private set
        {
            if (_isWeaponSelected == value)
                return;

            _isWeaponSelected = value;
            StateChanged?.Invoke();
        }
    }

    private void Start() =>
        _camera = Camera.main;

    public void SelectWeapon(Weapon weapon, Transform muzzle)
    {
        UnselectWeapon();

        CurrentWeapon = weapon;

        IsWeaponSelected = true;
        _muzzle = muzzle;

        CurrentWeapon.OnSelected(_camera);
    }

    public void UnselectWeapon()
    {
        IsWeaponSelected = false;
        CurrentWeapon = null;
        _muzzle = null;
    }

    public void TryShoot(Vector3 shootPosition)
    {
        if (!IsWeaponSelected || CurrentWeapon == null)
            return;

        if (!CurrentWeapon.RuntimeState.CanShoot)
            return;

        CurrentWeapon.Shoot(shootPosition, _bulletForce, _muzzle);
    }
}
