using UnityEngine;

public class Hud : MonoBehaviour
{
    [SerializeField] private LootCounter _lootCounter;
    [SerializeField] private HealthUI _healthUI;
    [SerializeField] private IndicateUI _indicateUI;
    [SerializeField] private InventoryView _inventoryView;

    public LootCounter LootCounter => _lootCounter;
    public HealthUI HealthUI => _healthUI;
    public IndicateUI IndicateUI => _indicateUI;
    public InventoryView InventoryView => _inventoryView;
}
