using UnityEngine;
using UnityEngine.UI;

public class PauseWindow : MonoBehaviour, IWindow
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private AudioSlider _audioSettings;
    [SerializeField] private AudioClip _closeButtonSound;

    private IPauseService _pause;
    private UIAudioHandler _audio;

    public AudioSlider AudioSettings => _audioSettings;

    public WindowsID WindowID => WindowsID.PauseWindow;

    private void OnEnable() => 
        _closeButton.onClick.AddListener(Hide);

    private void OnDisable() => 
        _closeButton.onClick.RemoveListener(Hide);

    private void Start() =>
        gameObject.SetActive(false);

    public void Construct(IPauseService pause, UIAudioHandler audioHandler)
    {
        _pause = pause;
        _audio = audioHandler;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _pause.Pause();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        _pause.Run();
        _audio.PlaySound(_closeButtonSound);
    }
}
