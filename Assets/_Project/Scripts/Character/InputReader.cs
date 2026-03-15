using UnityEngine;

public class InputReader : MonoBehaviour
{
    private bool _isShot;
    private bool _isReloadWeapon;

    public bool IsKeepWeapon { get; private set; }
    public bool IsRMBDown { get; private set; }

    private void Update()
    {
        IsRMBDown = Input.GetMouseButton(1);

        if (Input.GetKeyDown(KeyCode.E))
            IsKeepWeapon = !IsKeepWeapon;

        if (Input.GetMouseButtonDown(0))
            _isShot = true;

        if (Input.GetKeyDown(KeyCode.R))
            _isReloadWeapon = true;
    }

    public bool GetIsShot() => GetBoolIsTrigger(ref _isShot);

    public bool GetIsReloadWeapon() => GetBoolIsTrigger(ref _isReloadWeapon);

    private bool GetBoolIsTrigger(ref bool value)
    {
        bool localValue = value;
        value = false;
        return localValue;
    }
}
