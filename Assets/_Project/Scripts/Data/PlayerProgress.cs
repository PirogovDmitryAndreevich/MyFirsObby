using System;

[Serializable]
public class PlayerProgress
{
    public string MainScene = "Main";

    public PlayerState PlayerState;
    public Arsenal Arsenal;
    public BuildProgress BuildProgress;
    public ÑonveerProgress Conveers;

    public PlayerProgress(IStaticDataService staticData)
    {
        PlayerState = new PlayerState();
        Arsenal = new Arsenal(staticData);
        BuildProgress = new BuildProgress();
        Conveers = new ÑonveerProgress(staticData);
    }
}

[Serializable]
public class BuildProgress
{
    public bool IsWallOpened { get; private set; }
    public bool IsFloorOpened { get; private set; }

    public BuildProgress()
    {
        IsWallOpened = false;
        IsFloorOpened = false;
    }

    public void OpenWall() => 
        IsWallOpened = true;

    public void OpenFloor() => 
        IsFloorOpened = true;
}