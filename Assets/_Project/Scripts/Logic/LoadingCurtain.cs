using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCurtain : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _progressText;
    [SerializeField] private float _speed = 1.5f;

    private Coroutine _animation;
    private float _targetValue;

    private void Awake() => 
        DontDestroyOnLoad(gameObject);

    private void OnEnable()
    {
        _animation = StartCoroutine(AnimateSlider());
    }

    private void OnDisable()
    {
        if (_animation != null)
            StopCoroutine(_animation);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.alpha = 1;
        _slider.value = 0;
        _targetValue = 0;

        UpdateProgressText(0);
    }

    public void SetProgress(float value) => 
        _targetValue = value;

    public void Hide()
    {
        _canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }

    private IEnumerator AnimateSlider()
    {
        while (true)
        {
            _slider.value = Mathf.MoveTowards(
                _slider.value,
                _targetValue,
                _speed * Time.deltaTime);

            UpdateProgressText(_slider.value);

            yield return null;
        }
    }

    private void UpdateProgressText(float progress)
    {
        int percent = Mathf.RoundToInt(progress * 100f);
        _progressText.text = $"{percent}%";
    }
}
