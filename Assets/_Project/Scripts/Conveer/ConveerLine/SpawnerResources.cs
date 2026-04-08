using UnityEngine;

public class SpawnerResources : MonoBehaviour
{
    [SerializeField] private ConveerResource _resource;
    [SerializeField] private ConveerCubeMover _cubeMover;

    private ConveerLine _line;

    private void Start() => 
        _cubeMover.Create += Spawn;

    private void OnDestroy() =>
        _cubeMover.Create -= Spawn;

    public void Construct(ConveerLine line) => 
        _line = line;

    private void Spawn()
    {
        ConveerResource resource = Instantiate(_resource, _cubeMover.ResourceSpawnPoint.position, Quaternion.identity);

        resource.SetModel
            (
            _line.LineStaticInfo.GetLevelInfo(_line.LineProgress.GetLevel(ConveerUpdateType.Level)).Prefab
            , _line.LineStaticInfo.GetLevelInfo(_line.LineProgress.CurrentLevel).Reward);
    }
}