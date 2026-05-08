using System;

[Serializable]
public class PlayerProgress
{
    public string MainScene = "Main";

    public PlayerState PlayerState;
    public WeaponsProgress Weapons;
    public BuildProgress BuildProgress;
    public AudioSettingSaveData Audio;

    public PlayerProgress(IStaticDataService staticData)
    {
        PlayerState = new PlayerState(staticData);
        Audio = new AudioSettingSaveData(staticData);
        BuildProgress = new BuildProgress();
        Weapons = new WeaponsProgress();
    }
}
