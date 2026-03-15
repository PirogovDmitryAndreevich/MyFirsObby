using UnityEngine;

public class MobileInputService : InputService
{
    public override Vector2 Axis =>
        SimpleInputAxis();

    public override Vector3 LookInputVector =>
        SimpleInputMouseAxis();

    public override bool IsCameraMoving
    {
        get
        {
            return SimpleInputMouseAxis() != Vector2.zero;
        }
    }
}
