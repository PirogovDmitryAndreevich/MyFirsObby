using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class AdNotification : MonoBehaviour
{
    [SerializeField] private TMP_Text _counter;
    [SerializeField] private float _duration = 2f;

    private Coroutine _timerCoroutine;
    private IPauseService _pause;

    public bool IsAdNotificationFinished { get; private set; } = true;

    public void Construct(IPauseService pause) => 
        _pause = pause;

    public void Show(Action onCompleted)
    {
        gameObject.SetActive(true);

        if (_timerCoroutine != null)
            StopCoroutine(_timerCoroutine);

        _timerCoroutine = StartCoroutine(ShowTimer(onCompleted));
        Debug.Log("Coroutine started");
    }

    public void Hide() => 
        gameObject.SetActive(false);

    private IEnumerator ShowTimer(Action onCompleted)
    {
        IsAdNotificationFinished = false;
        _pause.Pause();

        int timeLeft = Mathf.CeilToInt(_duration);

        while (timeLeft > 0)
        {
            _counter.text = timeLeft.ToString();

            yield return new WaitForSecondsRealtime(1f);

            timeLeft--;
        }

        yield return null;

        IsAdNotificationFinished = true;

        onCompleted?.Invoke();

        _pause.Run();
        Hide();
    }
}