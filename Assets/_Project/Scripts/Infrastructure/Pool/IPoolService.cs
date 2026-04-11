using System;
using UnityEngine;

public interface IPoolService : IService
{
    void RegisterFactory<TPoolObject>(Func<TPoolObject> factory) where TPoolObject : Component;
    TPoolObject Get<TPoolObject>() where TPoolObject : Component;
    void Return<TPoolObject>(TPoolObject obj) where TPoolObject : Component;
}
