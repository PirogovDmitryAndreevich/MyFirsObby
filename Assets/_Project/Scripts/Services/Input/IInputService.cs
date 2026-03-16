
using UnityEngine;

public interface IInputService : IService
{
    public Vector2 Axis { get; }

    public Vector3 LookInputVector { get; }

    public bool IsCameraMoving { get; }

    public bool IsAttackButtonUp();

    public bool IsInteractButtonUp();
}
