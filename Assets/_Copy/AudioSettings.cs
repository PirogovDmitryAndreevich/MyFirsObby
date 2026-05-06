using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _spriteOn;
    [SerializeField] private Sprite _spriteOff;

    public event Action<float> ValueChanged;

    private AudioSettingSaveData _progressData;

    private void OnEnable()
    {
        _button.onClick.AddListener(SwitchMute);
        _slider.onValueChanged.AddListener(ChangedSoundVolume);
        InitializeView();
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(SwitchMute);
        _slider.onValueChanged.RemoveListener(ChangedSoundVolume);

        _progressData.MuteChanged -= SetButtonView;
    }

    public void Construct(AudioSettingSaveData progressData)
    {
        _progressData = progressData;
        InitializeView();

        _progressData.MuteChanged += SetButtonView;
    }

    private void InitializeView()
    {
        _image.sprite = _progressData.IsMute ? _spriteOff : _spriteOn;
        _slider.SetValueWithoutNotify(_progressData.Volume);
    }

    private void ChangedSoundVolume(float value)
    {
        float finalValue = Mathf.Clamp01(value);
        _progressData.ChangeVolume(finalValue);
    }

    private void SwitchMute() => 
        _progressData.SwitchMute();

    private void SetButtonView() =>
        _image.sprite = _progressData.IsMute ? _spriteOff : _spriteOn;
}