using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioHandler _audioHandler;
    [SerializeField] private AudioClip _backgroundMusic;

    private void Start() => 
        _audioHandler.PlayMusic(_backgroundMusic);
}
