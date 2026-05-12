using UnityEngine;

public class EffectAudioHandler : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private AudioSettingSaveData _settingSaveData;
    private bool _isInitialize;

    private void OnEnable()
    {
        Debug.Log("EffectAudioHandler OnEnable");
        if (_isInitialize)
        {
            ResetVolume();
            ResetMute();
        }
    }

    public void Construct(AudioSettingSaveData audioSettingSaveData)
    {
        Debug.Log("EffectAudioHandler construct");
        _settingSaveData = audioSettingSaveData;
        Initialize();
    }

    public void PlaySound(AudioClip effectSound) =>
        _audioSource.PlayOneShot(effectSound);

    private void Initialize()
    {
        _isInitialize = true;

        ResetVolume();
        ResetMute();
    }

    private void ResetVolume() =>
        _audioSource.volume = _settingSaveData.Volume;

    private void ResetMute() =>
        _audioSource.mute = _settingSaveData.IsMute;
}