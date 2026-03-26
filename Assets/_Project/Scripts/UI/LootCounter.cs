using TMPro;
using UnityEngine;

public class LootCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _counterText;

    private PlayerResources _resources;

    private void Start()
    {
        UpdateCounter();
    }

    private void OnDestroy() => 
        _resources.ValueChanged -= UpdateCounter;

    public void Construct(PlayerResources resources)
    {
        _resources = resources;
        _resources.ValueChanged += UpdateCounter;
    }

    private void UpdateCounter()
    {
        _counterText.text = $"{_resources.Cash}";
    }
}