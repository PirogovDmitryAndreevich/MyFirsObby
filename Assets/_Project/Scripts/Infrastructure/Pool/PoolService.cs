using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolService : IPoolService
{
    private readonly Dictionary<Type, Queue<Component>> _pools = new();

    private readonly Dictionary<Type, Func<Component>> _factories = new();

    public void RegisterFactory<TPoolObject>(Func<TPoolObject> factory) where TPoolObject : Component => 
        _factories[typeof(TPoolObject)] = () => factory();

    public TPoolObject Get<TPoolObject>() where TPoolObject : Component
    {
        Type type = typeof(TPoolObject);

        if (_pools.TryGetValue(type, out var pool) && pool.Count > 0)
        {
            TPoolObject obj = (TPoolObject)pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        return CreateNew<TPoolObject>();
    }

    public void Return<TPoolObject>(TPoolObject obj) where TPoolObject : Component
    {
        Type type = typeof(TPoolObject);

        if (!_pools.ContainsKey(type))
            _pools[type] = new Queue<Component>();

        obj.gameObject.SetActive(false);
        _pools[type].Enqueue(obj);
    }

    private TPoolObject CreateNew<TPoolObject>() where TPoolObject : Component
    {
        if (!_factories.TryGetValue(typeof(TPoolObject), out var factory))
            throw new Exception($"No factory for {typeof(TPoolObject)}");

        return (TPoolObject)factory();
    }
}