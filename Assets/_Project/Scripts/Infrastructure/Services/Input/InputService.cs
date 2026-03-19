using UnityEngine;

public abstract class InputService : IInputService
{
    private const string AttackButton = "Attack";
    private const string InteractButton = "Interact";
    private const string AttackStateButton = "AttackState";

    private bool _isKeepWeapon = false;
    private bool _attackStatePressed;

    public Vector2 ShootPosition { get; protected set; }

    public abstract Vector2 Axis { get; }

    public Vector3 LookInputVector
    {
        get
        {
            Vector2 axis = SimpleInputMouseAxis();

            return axis;
        }
    }

    public bool IsCameraMoving =>
        LookInputVector != Vector3.zero;

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

    protected abstract bool IsAttackStateButtonDown();    

    protected bool IsInteractButtonUp() =>
        SimpleInput.GetButtonDown(InteractButton);

    protected bool AttackState() =>
        SimpleInput.GetButtonDown(AttackStateButton);

    protected Vector2 SimpleInputAxis() =>
        new Vector2
        (
            SimpleInput.GetAxis(ConstantsData.InputAxis.HorizontalAxis),
            SimpleInput.GetAxis(ConstantsData.InputAxis.VerticalAxis)
        );

    private Vector2 SimpleInputMouseAxis() =>
        new Vector2
        (
            SimpleInput.GetAxis(ConstantsData.InputAxis.MouseRigth),
            SimpleInput.GetAxis(ConstantsData.InputAxis.MouseUp)
        );

    private void SwitchState() => 
        _isKeepWeapon = !_isKeepWeapon;
}