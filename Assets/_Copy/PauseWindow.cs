using UnityEngine;
using UnityEngine.UI;

public class PauseWindow : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private AudioSettings _audioSettings;
    private IPauseService _pause;

    public AudioSettings AudioSettings => _audioSettings;

    private void OnEnable() => 
        _closeButton.onClick.AddListener(Hide);

    private void OnDisable() => 
        _closeButton.onClick.RemoveListener(Hide);

    private void Start() => 
        Hide();

    public void Construct(IPersistentProgressService progress, IPauseService pause)
    {
        AudioSettings.Construct(progress.Progress.Audio);
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
