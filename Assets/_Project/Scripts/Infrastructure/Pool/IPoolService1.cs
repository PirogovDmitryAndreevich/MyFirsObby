using System;
using UnityEngine;

public interface IPoolService1
{
    TPoolObject Get<TPoolObject>() where TPoolObject : Component;
    void RegisterFactory<TPoolObject>(Func<TPoolObject> factory) where TPoolObject : Component;
    void Return<TPoolObject>(TPoolObject obj) where TPoolObject : Component;
}