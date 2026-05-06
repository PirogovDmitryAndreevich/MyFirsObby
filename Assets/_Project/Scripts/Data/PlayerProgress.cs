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

public class AudioSettingSaveData
{
    public bool IsMute;
    public float Volume;

    public event Action VolumeChanged;
    public event Action MuteChanged;

    public AudioSettingSaveData(IStaticDataService staticData)
    {
        IsMute = false;
        Volume = 1f;
    }

    public void ChangeVolume(float value)
    {
        Volume = value;

        bool shouldBeMuted = Volume <= 0;

        if (IsMute != shouldBeMuted)
            SwitchMute();

        VolumeChanged?.Invoke();
    }

    public void SwitchMute()
    {
        IsMute = !IsMute;
        MuteChanged?.Invoke();
    }
}