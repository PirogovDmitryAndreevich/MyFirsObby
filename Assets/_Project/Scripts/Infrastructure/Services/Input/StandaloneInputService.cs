using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class StandaloneInputService : InputService
{
    public override Vector2 Axis
    {
        get
        {
            Vector2 axis = SimpleInputAxis();

            if (axis == Vector2.zero)
                axis = UnityAxis();

            return axis;
        }
    }

    public override Vector3 LookInputVector
    {
        get
        {
            Vector2 axis = SimpleInputMouseAxis();

            if (axis == Vector2.zero)
                axis = UnityMouseAxis();

            return axis;
        }
    }

    public override bool IsCameraMoving
    {
        get
        {
            Vector2 axis = SimpleInputMouseAxis();

            if (axis != Vector2.zero)
                return true;
            else
                return Input.GetMouseButton(1);
        }
    }

    private Vector2 UnityAxis() =>
        new Vector2
        (
            Input.GetAxis(ConstantsData.InputAxis.HorizontalAxis),
            Input.GetAxis(ConstantsData.InputAxis.VerticalAxis)
        );

    private Vector2 UnityMouseAxis() =>
        new Vector2
        (
            Input.GetAxis(ConstantsData.InputAxis.MouseX),
            Input.GetAxis(ConstantsData.InputAxis.MouseY)
        );
}