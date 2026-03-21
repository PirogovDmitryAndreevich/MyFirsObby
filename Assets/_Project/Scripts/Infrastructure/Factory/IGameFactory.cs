using System;
using System.Collections.Generic;
using UnityEngine;

public interface IGameFactory : IService
{
    List<ISavedProgress> ProgressWriters { get; }
    List<ISavedProgressReader> ProgressReaders { get; }

    void Cleanup();
    GameObject CreateCharacter(GameObject at);
    GameObject CreateHod(bool isMobile);
    LootPiece CreateLoot();
    void CreateMonster(MonsterTypeID monsterTypeID, Transform parent);
    void Register(ISavedProgressReader progressReader);
}