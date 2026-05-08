using UnityEngine;

public class CharacterSound : MonoBehaviour
{
    [SerializeField] private AudioHandler _handler;
    [SerializeField] private AudioClip _buildStepSound;
    [SerializeField] private AudioClip _grassStepSound;
    [SerializeField] private AudioClip _jumpSound;

    private float _nextPlayStepTime;

    public void PlayStepSound() =>
        _handler.PlayRandomPithSound(_buildStepSound, ref _nextPlayStepTime);

    public void PlayJumpSound() =>
        _handler.PlaySound(_jumpSound);
}
