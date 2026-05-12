using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _spriteOn;
    [SerializeField] private Sprite _spriteOff;
    [SerializeField] private AudioClip _sliderSound;
    [SerializeField] private AudioClip _mutButtonSound;

    private AudioSettingSaveData _progressData;
    private UIAudioHandler _audio;
    private ISaveLoadService _save;
    private bool _isInitialized = false;

    private void OnEnable()
    {
        _button.onClick.AddListener(SwitchMute);
        _slider.onValueChanged.AddListener(ChangedSoundVolume);

        if (_isInitialized)
            InitializeView();
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(SwitchMute);
        _slider.onValueChanged.RemoveListener(ChangedSoundVolume);

        _progressData.MuteChanged -= SetButtonView;
    }

    private void Start()
    {
        InitializeView();
        _isInitialized = true;

        _progressData.MuteChanged += SetButtonView;
    }

    public void Construct(AudioSettingSaveData progressData, UIAudioHandler audioHandler, ISaveLoadService save)
    {
        _progressData = progressData;
        _audio = audioHandler;
        _save = save;
    }

    private void InitializeView()
    {
        SetButtonView();
        _slider.SetValueWithoutNotify(_progressData.Volume);
    }

    private void ChangedSoundVolume(float value)
    {
        float finalValue = Mathf.Clamp01(value);
        _audio.PlaySound(_sliderSound);
        _progressData.ChangeVolume(finalValue);
        _save.SaveProgress();
    }

    private void SwitchMute()
    {
        _audio.PlaySound(_mutButtonSound);
        _progressData.SwitchMute();
        SetButtonView();
        _save.SaveProgress();
    }

    private void SetButtonView() =>
        _image.sprite = _progressData.IsMute ? _spriteOff : _spriteOn;
}