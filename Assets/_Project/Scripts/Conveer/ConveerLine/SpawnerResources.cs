using UnityEngine;

public class SpawnerResources : MonoBehaviour
{
    [SerializeField] private ConveerCubeMover _cubeMover;

    private IConveyersPool _pool;
    private ConveyerType _type;

    private void OnDestroy() => 
        _cubeMover.Create -= Spawn;

    public void Construct(IConveyersPool pool, ConveyerType type)
    {
        _pool = pool;
        _type = type;

        _cubeMover.Create += Spawn;
        Debug.Log("Subscribed to Create");
    }

    private void Spawn()
    {
        Debug.Log("Spawn called");

        ConveerResource resource = _pool.Get(_type);

        Debug.Log($"Got resource: {resource}");

        resource.ResetState(_cubeMover.ResourceSpawnPoint.position);
    }
}