using UnityEngine;

public abstract class InputService : IInputService
{
    private const string InteractButton = "Interact";
    private const string AttackStateButton = "AttackState";

    private bool _isKeepWeapon = false;
    private bool _attackStatePressed;

    public Vector2 ShootPosition { get; protected set; }

    public abstract Vector2 Axis { get; }

    public abstract Vector3 LookInputVector { get; }    

    public abstract bool IsCameraMoving { get; }    

    public abstract bool IsShoot {  get; }   

    public bool GetAttackState()
    {
        if (IsAttackStateButtonDown())
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

    protected bool IsAttackStateButtonDown() =>
        AttackState();

    protected bool IsInteractButtonUp() =>
        SimpleInput.GetButtonDown(InteractButton);

    protected Vector2 SimpleInputAxis() =>
        new Vector2
        (
            SimpleInput.GetAxis(ConstantsData.InputAxis.HorizontalAxis),
            SimpleInput.GetAxis(ConstantsData.InputAxis.VerticalAxis)
        );

    protected Vector2 SimpleInputMouseAxis() =>
        new Vector2
        (
            SimpleInput.GetAxis(ConstantsData.InputAxis.MouseRigth),
            SimpleInput.GetAxis(ConstantsData.InputAxis.MouseUp)
        );

    private void SwitchState() => 
        _isKeepWeapon = !_isKeepWeapon;

    private bool AttackState() =>
        SimpleInput.GetButtonDown(AttackStateButton);

}