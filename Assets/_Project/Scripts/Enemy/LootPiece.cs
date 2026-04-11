using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LootPiece : MonoBehaviour
{
    private PlayerState _resources;
    private ISaveLoadService _save;
    private IPoolService _poolService;
    private Loot _loot;
    private bool _picked;
    public Collider Collider { get; private set; }

    public void Construct(PlayerState resources, ISaveLoadService saveLoadService, IPoolService poolService)
    {
        _poolService = poolService;
        _resources = resources;
        _save = saveLoadService;

        Collider = GetComponent<Collider>();
    }

    public void Initialize(Loot loot)
    {
        _picked = false;
        _loot = loot;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character character))
            Pickup();
    }

    private void Pickup()
    {
        if (_picked)
            return;

        _picked = true;

        _resources.AddCash(_loot.Value);

        _save.SaveProgress();
        _poolService.Return(this);
    }
}