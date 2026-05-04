using System;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    private Camera _camera;
    private bool _isWeaponSelected;

    public event Action StateChanged;

    public WeaponBase CurrentWeapon { get; private set; }
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

    private void Update()
    {
        if (!IsWeaponSelected || CurrentWeapon == null)
            return;

        Vector3 aimPoint = GetAimPoint(Input.mousePosition);

        Debug.DrawLine(CurrentWeapon.Muzzle.position, aimPoint, Color.blue);
    }

    public void SelectWeapon(WeaponBase weapon)
    {
        UnselectWeapon();

        CurrentWeapon = weapon;

        IsWeaponSelected = true;
    }

    public void UnselectWeapon()
    {
        IsWeaponSelected = false;
        CurrentWeapon = null;
    }

    public void TryShoot(Vector3 shootPosition)
    {
        if (!IsWeaponSelected || CurrentWeapon == null)
            return;

        if (!CurrentWeapon.CanShoot)
            return;

        Vector3 aimPoint = GetAimPoint(shootPosition);

        Vector3 direction = (aimPoint - CurrentWeapon.Muzzle.position).normalized;

        CurrentWeapon.Shoot(direction);
    }

    private Vector3 GetAimPoint(Vector3 screenPosition)
    {
        Ray ray = _camera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, CurrentWeapon.Distance))
        {
            if (Vector3.Distance(hit.point, _camera.transform.position) > 10f)
                return hit.point;
        }

        Vector3 farPoint = ray.origin + ray.direction * CurrentWeapon.Distance;

        return farPoint;
    }
}
