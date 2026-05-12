using UnityEngine;

public class AudioHandler : MonoBehaviour, ISavedProgressReader
{
    [SerializeField] private AudioSource _audioSource;    

    protected AudioSettingSaveData SettingSaveData;

    private void OnDestroy()
    {
        SettingSaveData.VolumeChanged -= ResetVolume;
        SettingSaveData.MuteChanged -= ResetMute;
    }

    public void LoadProgress(PlayerProgress progress)
    {
        SettingSaveData = progress.Audio;

        SettingSaveData.VolumeChanged += ResetVolume;
        SettingSaveData.MuteChanged += ResetMute;

        ResetVolume();
        ResetMute();
    }

    public void PlayMusic(AudioClip clip)
    {
        _audioSource.loop = true;
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void PlaySound(AudioClip clip) => 
        _audioSource.PlayOneShot(clip);

    protected virtual void ResetVolume() => 
        _audioSource.volume = SettingSaveData.Volume;

    protected virtual void ResetMute() => 
        _audioSource.mute = SettingSaveData.IsMute;
}
