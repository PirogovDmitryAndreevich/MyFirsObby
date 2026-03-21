using UnityEngine;

public class LootPiece : MonoBehaviour
{
    private PlayerResources _resources;
    private Loot _loot;
    private bool _picked;

    public void Construct(PlayerResources resources)
    {
        _resources = resources;
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

        Debug.Log("Loot Collected");
        Destroy(gameObject);
    }
}