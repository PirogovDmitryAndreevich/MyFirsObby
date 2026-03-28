using UnityEngine;

public class LootPiece : MonoBehaviour
{
    private PlayerState _resources;
    private ISaveLoadService _save;
    private Loot _loot;
    private bool _picked;

    public void Construct(PlayerState resources, ISaveLoadService saveLoadService)
    {
        _resources = resources;
        _save = saveLoadService;
    }

    public void Initialize(Loot loot)
    {
        _loot = loot;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CharacterController character))
            Pickup();
    }

    private void Pickup()
    {
        if (_picked)
            return;

        _picked = true;

        _resources.Collect(_loot);

        _save.SaveProgress();
        Destroy(gameObject);
    }
}