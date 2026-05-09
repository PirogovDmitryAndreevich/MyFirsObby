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
    }

    private void Spawn()
    {
        ConveyerResource resource = _pool.Get(_type);
        resource.transform.rotation = transform.rotation;
        resource.ResetState(_cubeMover.ResourceSpawnPoint.position);
    }
}