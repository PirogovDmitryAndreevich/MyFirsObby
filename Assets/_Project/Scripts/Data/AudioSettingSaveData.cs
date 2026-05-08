using System;

[Serializable]
public class AudioSettingSaveData
{
    public bool IsMute;
    public float Volume;

    public event Action VolumeChanged;
    public event Action MuteChanged;

    public AudioSettingSaveData(IStaticDataService staticData)
    {
        IsMute = false;
        Volume = 0.5f;
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