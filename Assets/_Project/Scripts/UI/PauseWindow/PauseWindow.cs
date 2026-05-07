using UnityEngine;
using UnityEngine.UI;

public class PauseWindow : MonoBehaviour, IWindow
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private AudioSlider _audioSettings;

    private IPauseService _pause;

    public AudioSlider AudioSettings => _audioSettings;

    public WindowsID WindowID => WindowsID.PauseWindow;

    private void OnEnable() => 
        _closeButton.onClick.AddListener(Hide);

    private void OnDisable() => 
        _closeButton.onClick.RemoveListener(Hide);

    private void Start() => 
        Hide();

    public void Construct(IPauseService pause)
    {
        _pause = pause;
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
    }
}
