using System.Collections;
using TMPro;
using UnityEngine;

public class DamageImpactPiece : MonoBehaviour
{
    private const float Duration = 1.5f;
    private const float Height = 1f;
    private const float HorizontalOffset = 0.5f;
    private const float WaveStrength = 0.03f;
    private const float WaveFrequency = 15f;

    [SerializeField] private TMP_Text _text;

    private IPoolService _poolService;
    private Coroutine _animationCoroutine;

    public void Construct(IPoolService poolService) => 
        _poolService = poolService;

    public void Initialize(float value, Vector3 position)
    {
        int damage = Mathf.RoundToInt(value);
        _text.text = damage.ToString();

        if (_animationCoroutine != null)
            StopCoroutine(_animationCoroutine);

        _animationCoroutine = StartCoroutine(StartFlyingUp(position));
    }

    private IEnumerator StartFlyingUp(Vector3 position)
    {
        transform.position = position;

        Vector3 startPosition = position;

        float randomX = Random.Range(-HorizontalOffset, HorizontalOffset);

        Vector3 targetPosition = startPosition +
                                 new Vector3(randomX, Height, 0f);

        float elapsed = 0f;

        while (elapsed < Duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / Duration;

            transform.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                EaseOutCubic(t));

            transform.position += Vector3.right *
                                  Mathf.Sin(t * WaveFrequency) *
                                  WaveStrength;

            yield return null;
        }

        _poolService.Return(this);
    }

    private float EaseOutCubic(float t)
    {
        return 1f - Mathf.Pow(1f - t, 3f);
    }
}