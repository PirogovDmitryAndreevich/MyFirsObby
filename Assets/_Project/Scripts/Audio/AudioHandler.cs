using UnityEngine;

public class AudioHandler : MonoBehaviour, ISavedProgressReader
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _randomPitchSoundSource;
    [SerializeField] private float _lowPith = 0f;
    [SerializeField] private float _topPith = 2f;

    private AudioSettingSaveData _settingSaveData;

    private void OnDestroy()
    {
        _settingSaveData.VolumeChanged -= ResetVolume;
        _settingSaveData.MuteChanged -= ResetMute;
    }

    public void LoadProgress(PlayerProgress progress)
    {
        _settingSaveData = progress.Audio;

        _settingSaveData.VolumeChanged += ResetVolume;
        _settingSaveData.MuteChanged += ResetMute;

        ResetVolume();
        ResetMute();
    }

    public void PlaySound(AudioClip clip) => 
        _audioSource.PlayOneShot(clip);

    public void PlayRandomPithSound(AudioClip clip, ref float nextPlayTime)
    {
        if (nextPlayTime < Time.time)
        {
            _audioSource.pitch = Random.Range(_lowPith, _topPith);
            _audioSource.PlayOneShot(clip);
            nextPlayTime = clip.length + Time.time;
        }
    }

    private void ResetVolume()
    {
        _audioSource.volume = _settingSaveData.Volume;
        _randomPitchSoundSource.volume = _settingSaveData.Volume;
    }

    private void ResetMute()
    {
        _audioSource.mute = _settingSaveData.IsMute;
        _randomPitchSoundSource.mute = _settingSaveData.IsMute;
    }
}
