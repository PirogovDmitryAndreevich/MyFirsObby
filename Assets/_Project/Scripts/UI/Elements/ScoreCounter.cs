using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _counterText;

    private PlayerState _resources;

    private void Start()
    {
        UpdateCounter();
    }

    private void OnDestroy() =>
        _resources.CashChanged -= UpdateCounter;

    public void Construct(PlayerState resources)
    {
        _resources = resources;
        _resources.ScoreChanged += UpdateCounter;
    }

    private void UpdateCounter()
    {
        _counterText.text = $"{_resources.Score}";
    }
}