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

    private void Update()
    {
        if (!IsWeaponSelected || CurrentWeapon == null || _muzzle == null)
            return;

        Vector3 aimPoint = GetAimPoint(Input.mousePosition);

        Debug.DrawLine(_muzzle.position, aimPoint, Color.blue);
    }

    public void SelectWeapon(Weapon weapon, Transform muzzle)
    {
        UnselectWeapon();

        CurrentWeapon = weapon;

        IsWeaponSelected = true;
        _muzzle = muzzle;
    }

    public void UnselectWeapon()
    {
        IsWeaponSelected = false;
        CurrentWeapon = null;
        _muzzle = null;
    }

    public void TryShoot(Vector3 shootPosition)
    {
        if (!IsWeaponSelected || CurrentWeapon == null || _muzzle == null)
            return;

        if (!CurrentWeapon.RuntimeState.CanShoot)
            return;

        Vector3 aimPoint = GetAimPoint(shootPosition);

        CurrentWeapon.Shoot(aimPoint, _bulletForce, _muzzle);
    }

    private Vector3 GetAimPoint(Vector3 screenPosition)
    {
        Ray ray = _camera.ScreenPointToRay(screenPosition);

        Vector3 farPoint = ray.origin + ray.direction * CurrentWeapon.StaticData.Distance;

        if (Physics.Raycast(ray, out RaycastHit hit, CurrentWeapon.StaticData.Distance))
        {
            if (Vector3.Distance(hit.point, _camera.transform.position) > 10f)
                return hit.point;
        }

        return farPoint;
    }
}
