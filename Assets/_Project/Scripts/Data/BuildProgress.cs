using System;

[Serializable]
public class BuildProgress
{
    public bool IsWallOpened;
    public bool IsFloorOpened;

    public event Action WallOpened;
    public event Action FloorOpened;

    public BuildProgress()
    {
        IsWallOpened = false;
        IsFloorOpened = false;
    }

    public void OpenWall()
    {
        IsWallOpened = true;
        WallOpened?.Invoke();
    }

    public void OpenFloor()
    {
        IsFloorOpened = true;
        FloorOpened?.Invoke();
    }
}