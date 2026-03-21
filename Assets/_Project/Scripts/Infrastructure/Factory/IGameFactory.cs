using System;
using System.Collections.Generic;
using UnityEngine;

public interface IGameFactory : IService
{
    List<ISavedProgress> ProgressWriters { get; }
    List<ISavedProgressReader> ProgressReaders { get; }

    void Cleanup();
    GameObject CreateCharacter(GameObject at);
    GameObject CreateHudMobile();
    GameObject CreateHudDesktop();
    void CreateMonster(MonsterTypeID monsterTypeID, Transform parent);
    void RegisterProgressWatchers(GameObject gameObject);
    void Register(ISavedProgressReader progressReader);
}