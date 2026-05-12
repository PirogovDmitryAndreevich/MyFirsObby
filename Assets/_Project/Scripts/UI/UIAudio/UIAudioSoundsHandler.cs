using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UIAudioHandler : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;

    private AudioSettingSaveData _settingSaveData;

    private void OnDestroy()
    {
        _settingSaveData.VolumeChanged -= ResetVolume;
        _settingSaveData.MuteChanged -= ResetMute;
    }

    public void Construct(PlayerProgress progress)
    {
        _settingSaveData = progress.Audio;

        _settingSaveData.VolumeChanged += ResetVolume;
        _settingSaveData.MuteChanged += ResetMute;

        ResetVolume();
        ResetMute();
    }

    public void PlaySound(AudioClip clip) =>
            _audioSource.PlayOneShot(clip);

    private void ResetVolume() =>
        _audioSource.volume = _settingSaveData.Volume;

    private void ResetMute() =>
        _audioSource.mute = _settingSaveData.IsMute;
}