using SimpleInputNamespace;
using UnityEngine;

public abstract class InputService : IInputService
{
    private const string AttackButton = "Attack";
    private const string InteractButton = "Interact";

    public abstract Vector2 Axis { get; }
    public abstract Vector3 LookInputVector { get; }
    public abstract bool IsCameraMoving { get; }

    public bool IsAttackButtonUp() =>
        SimpleInput.GetButtonDown(AttackButton);

    public bool IsInteractButtonUp() =>
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
}