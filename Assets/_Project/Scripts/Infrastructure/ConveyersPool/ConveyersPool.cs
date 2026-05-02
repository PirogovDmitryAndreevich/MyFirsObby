using System.Collections.Generic;
using UnityEngine;

public class ConveyersPool : IConveyersPool
{
    private readonly IStaticDataService _staticData;

    private Dictionary<ConveyerType, Queue<ConveerResource>> _resources;

    public ConveyersPool(IStaticDataService staticData)
    {
        _resources = new();
        _staticData = staticData;
    }

    public ConveerResource Get(ConveyerType type)
    {
        if (_resources.TryGetValue(type, out var queue) && queue.Count > 0)
        {
            ConveerResource resource = queue.Dequeue();
            resource.gameObject.SetActive(true);
            return resource;
        }

        return CreateNew(type);
    }

    public void Return(ConveerResource resource)
    {
        if (!_resources.ContainsKey(resource.Type))
            _resources[resource.Type] = new Queue<ConveerResource>();

        resource.gameObject.SetActive(false);
        _resources[resource.Type].Enqueue(resource);
    }

    private ConveerResource CreateNew(ConveyerType type)
    {
        if (!_resources.ContainsKey(type))
            _resources[type] = new Queue<ConveerResource>();

        ConveerResource resource = Object.Instantiate(
    _staticData.ForConveyer(type).Resource);

        resource.Construct(this);
        resource.gameObject.SetActive(true);

        return resource;
    }
}