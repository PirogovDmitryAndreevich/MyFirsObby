using UnityEngine;

public abstract class InputService : IInputService
{    
    private const string AttackStateButton = "AttackState";

    private bool _isKeepWeapon = false;
    private bool _attackStatePressed;

    public Vector2 ShootPosition { get; protected set; }

    public abstract Vector2 Axis { get; }

    public abstract Vector3 LookInputVector { get; }

    public abstract float ZoomInput { get; }

    public abstract bool IsCameraMoving { get; }    

    public abstract bool IsShoot {  get; }

    public abstract bool JumpPressed { get; }

    public bool GetAttackState()
    {
        if (IsActionButtonDown())
        {
            if (!_attackStatePressed)
            {
                SwitchState();
                _attackStatePressed = true;
            }
        }
        else
        {
            _attackStatePressed = false;
        }

        return _isKeepWeapon;
    }

    public bool GetInteract()
    {
        return true;
    }

    protected bool IsActionButtonDown() =>
        IsActionButton();

    public abstract bool GetInteractButtonDown();       

    private void SwitchState() => 
        _isKeepWeapon = !_isKeepWeapon;

    private bool IsActionButton() =>
        SimpleInput.GetButtonDown(AttackStateButton);

}