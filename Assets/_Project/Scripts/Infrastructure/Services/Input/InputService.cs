using UnityEngine;

public abstract class InputService : IInputService
{
    public Vector2 ShootPosition { get; protected set; }

    public abstract Vector2 Axis { get; }

    public abstract Vector3 LookInputVector { get; }

    public abstract float ZoomInput { get; }

    public abstract bool IsCameraMoving { get; }    

    public abstract bool IsShoot {  get; }

    public abstract bool JumpPressed { get; }

    public bool GetInteract()
    {
        return true;
    }

    public abstract bool GetInteractButtonDown();       
}