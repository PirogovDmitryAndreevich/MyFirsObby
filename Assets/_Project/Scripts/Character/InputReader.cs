using UnityEngine;

public class InputReader : MonoBehaviour
{
    private float _movementX;
    private float _movementY;
    private bool _isShot;

    public bool IsKeepWeapon { get; set; }
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
    }

    public bool GetIsShot() => GetBoolIsTrigger(ref _isShot);

    private bool GetBoolIsTrigger(ref bool value)
    {
        bool localValue = value;
        value = false;
        return localValue;
    }
}
