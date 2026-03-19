
using UnityEngine;

public interface IInputService : IService
{
    public Vector2 Axis { get; }

    public Vector3 LookInputVector { get; }

    public Vector2 ShootPosition { get; }

    public bool IsCameraMoving { get; } 

    public bool IsShoot {  get; }

    public bool GetAttackState();

    public bool GetInteract();
}
