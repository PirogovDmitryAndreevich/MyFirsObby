using UnityEngine;

public class CharacterSound : MonoBehaviour
{
    [SerializeField] private PithAudioHandler _handler;
    [SerializeField] private AudioClip _stepSound;
    [SerializeField] private AudioClip _jumpSound;

    private float _nextPlayStepTime;

    public void PlayStepSound() =>
        _handler.PlayRandomPithSound(_stepSound, ref _nextPlayStepTime);

    public void PlayJumpSound() =>
        _handler.PlayRandomPithSound(_jumpSound);
}
