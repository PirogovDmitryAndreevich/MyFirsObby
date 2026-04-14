using UnityEngine;
using UnityEngine.EventSystems;

public class StandaloneInputService : InputService
{
    public override bool IsCameraMoving =>
        Input.GetMouseButton(1);

    public override Vector2 Axis =>
        UnityAxis();

    public override Vector3 LookInputVector =>
        MouseInputAxis();

    public override bool IsShoot
    {
        get
        {
            if (Input.GetMouseButton(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return false;

                ShootPosition = Input.mousePosition;
                return true;
            }

            return false;
        }
    }

    public override float ZoomInput =>
        Input.GetAxis(ConstantsData.InputAxis.MouseScroll);

    public override bool JumpPressed =>
        Input.GetKeyDown(KeyCode.Space);

    private Vector2 UnityAxis() =>
        new Vector2
        (
            Input.GetAxis(ConstantsData.InputAxis.HorizontalAxis),
            Input.GetAxis(ConstantsData.InputAxis.VerticalAxis)
        );

    private Vector3 MouseInputAxis() =>
        new Vector2
            (
                Input.GetAxis(ConstantsData.InputAxis.MouseX),
                Input.GetAxis(ConstantsData.InputAxis.MouseY)
            );

    public override bool GetInteractButtonDown() => 
        Input.GetKeyDown(KeyCode.E);
}