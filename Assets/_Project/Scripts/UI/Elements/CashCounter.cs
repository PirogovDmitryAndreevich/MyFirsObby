using TMPro;
using UnityEngine;

public class CashCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _counterText;

    private PlayerState _resources;

    private void Start()
    {
        UpdateCounter();
    }

    private void OnDestroy() => 
        _resources.ValueChanged -= UpdateCounter;

    public void Construct(PlayerState resources)
    {
        _resources = resources;
        _resources.ValueChanged += UpdateCounter;
    }

    private void UpdateCounter()
    {
        _counterText.text = $"{_resources.Cash}";
    }
}