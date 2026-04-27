using TMPro;
using UnityEngine;

public class BossTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private CanvasGroup _canvasGroup;

    private bool _isTimerNow;

    private void Start()
    {
        if (!_isTimerNow)
            gameObject.SetActive(false);
    }

    public void ShowBossTimer()
    {
        gameObject.SetActive(true);
        _isTimerNow = true;
    }

    public void HideBossTimer()
    {
        gameObject.SetActive(false);
        _isTimerNow = false;
    }

    public void UpdateBossTimer(float timerToBoss)
    {
        int totalSeconds = Mathf.CeilToInt(timerToBoss);

        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        _timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
