using System.Collections.Generic;
using UnityEngine;

public class ConveyersPool : IConveyersPool
{
    private readonly IStaticDataService _staticData;

    private Dictionary<ConveyerType, Queue<ConveyerResource>> _resources;

    public ConveyersPool(IStaticDataService staticData)
    {
        _resources = new();
        _staticData = staticData;
    }

    public ConveyerResource Get(ConveyerType type)
    {
        if (_resources.TryGetValue(type, out var queue) && queue.Count > 0)
        {
            ConveyerResource resource = queue.Dequeue();
            resource.gameObject.SetActive(true);
            return resource;
        }

        return CreateNew(type);
    }

    public void Return(ConveyerResource resource)
    {
        if (!_resources.ContainsKey(resource.Type))
            _resources[resource.Type] = new Queue<ConveyerResource>();

        resource.gameObject.SetActive(false);
        _resources[resource.Type].Enqueue(resource);
    }

    private ConveyerResource CreateNew(ConveyerType type)
    {
        if (!_resources.ContainsKey(type))
            _resources[type] = new Queue<ConveyerResource>();

        ConveyerResource resource = Object.Instantiate(
    _staticData.ForConveyer(type).Resource);

        resource.Construct(this);
        resource.gameObject.SetActive(true);

        return resource;
    }
}