using UnityEngine;

public class PithAudioHandler : AudioHandler
{
    [SerializeField] private AudioSource _randomPitchSoundSource;
    [SerializeField] private float _lowPith = 0f;
    [SerializeField] private float _topPith = 2f;

    public void PlayRandomPithSound(AudioClip clip, ref float nextPlayTime)
    {
        if (nextPlayTime < Time.time)
        {
            _randomPitchSoundSource.pitch = Random.Range(_lowPith, _topPith);
            _randomPitchSoundSource.PlayOneShot(clip);
            nextPlayTime = clip.length + Time.time;
        }
    }

    public void PlayRandomPithSound(AudioClip clip)
    {
        _randomPitchSoundSource.pitch = Random.Range(_lowPith, _topPith);
        _randomPitchSoundSource.PlayOneShot(clip);
    }

    protected override void ResetVolume()
    {
        base.ResetVolume();
        _randomPitchSoundSource.volume = SettingSaveData.Volume;
    }

    protected override void ResetMute()
    {
        base.ResetMute();
        _randomPitchSoundSource.mute = SettingSaveData.IsMute;
    }
}