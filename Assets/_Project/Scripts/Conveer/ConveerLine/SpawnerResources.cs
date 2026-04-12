using System.Collections.Generic;
using UnityEngine;

public class SpawnerResources : MonoBehaviour
{
    [SerializeField] private ConveerResource _resource;
    [SerializeField] private ConveerCubeMover _cubeMover;

    private ConveerLine _line;
    GameObject _model;
    private IPoolService _poolService;
    List<ConveerResource> _allResources = new();   

    private void Start() =>
        _cubeMover.Create += Spawn;

    private void OnDestroy()
    {
        _line.LineProgress.ConveerLineUpdated -= BuildResource;
        _cubeMover.Create -= Spawn;
    }

    public void Construct(ConveerLine line, IPoolService poolService)
    {
        _line = line;
        _poolService = poolService;
        _model = _line.LineStaticInfo.GetLevelInfo(_line.LineProgress.GetLevel(ConveerUpdateType.Level)).Prefab;

        _line.LineProgress.ConveerLineUpdated += BuildResource;
        _poolService.RegisterFactory(() => CreateResource());
    }

    private void BuildResource(ConveerUpdateType type)
    {
        if (type != ConveerUpdateType.Level)
            return;

        _model = _line.LineStaticInfo.GetLevelInfo(_line.LineProgress.GetLevel(ConveerUpdateType.Level)).Prefab;
    }

    private void Spawn()
    {
        int reward = _line.LineStaticInfo.GetLevelInfo(_line.LineProgress.CurrentLevel).Reward;

        ConveerResource resource = _poolService.Get<ConveerResource>();
        resource.transform.position = _cubeMover.ResourceSpawnPoint.position;

        resource.Init(reward, _model);
    }

    private ConveerResource CreateResource()
    {
        ConveerResource resource = Instantiate(_resource, _cubeMover.ResourceSpawnPoint.position, Quaternion.identity);
        resource.Construct(_poolService);
        _allResources.Add(resource);
        return resource;
    }
}