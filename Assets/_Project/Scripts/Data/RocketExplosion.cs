using UnityEngine;

public class RocketExplosion : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    public void SetAudioVolume(float volume, bool isMute)
    {
        _audioSource.volume = volume;
        _audioSource.mute = isMute;
    }
}
