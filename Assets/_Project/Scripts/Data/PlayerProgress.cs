using System;

[Serializable]
public class PlayerProgress
{
    public string MainScene = "Main";

    public PlayerState PlayerState;
    public WeaponsProgress Weapons;
    public BuildProgress BuildProgress;

    public PlayerProgress(IStaticDataService staticData)
    {
        PlayerState = new PlayerState(staticData);
        BuildProgress = new BuildProgress();
        Weapons = new WeaponsProgress();
    }
}
