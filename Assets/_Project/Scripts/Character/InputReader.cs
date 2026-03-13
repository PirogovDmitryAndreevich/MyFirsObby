using UnityEngine;

public class InputReader : MonoBehaviour
{
    private float _movementX;
    private float _movementY;
    private bool _isShot;
    private bool _isReloadWeapon;

    public bool IsKeepWeapon { get; private set; }
    public Vector2 Direction { get; private set; }

    private void Update()
    {
        _movementX = Input.GetAxis(ConstantsData.InputAxis.HorizontalAxis);
        _movementY = Input.GetAxis(ConstantsData.InputAxis.VerticalAxis);

        Direction = new Vector2(_movementX, _movementY);

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
