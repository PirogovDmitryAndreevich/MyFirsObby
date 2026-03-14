using UnityEngine;

public class InputReader : MonoBehaviour
{
    private float _movementX;
    private float _movementY;
    private float _mouseUp;
    private float _mouseRight;
    private bool _isShot;
    private bool _isReloadWeapon;

    public bool IsKeepWeapon { get; private set; }
    public bool IsRMBDown { get; private set; }
    public Vector3 Direction { get; private set; }
    public Vector3 LookInputVector { get; private set; }

    private void Update()
    {
        _movementX = Input.GetAxis(ConstantsData.InputAxis.HorizontalAxis);
        _movementY = Input.GetAxis(ConstantsData.InputAxis.VerticalAxis);

        _mouseUp = Input.GetAxis(ConstantsData.InputAxis.MouseY);
        _mouseRight = Input.GetAxis(ConstantsData.InputAxis.MouseX);

        Direction = new Vector3(_movementX, _movementY);
        LookInputVector = new Vector3(_mouseRight, _mouseUp, 0f);

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
