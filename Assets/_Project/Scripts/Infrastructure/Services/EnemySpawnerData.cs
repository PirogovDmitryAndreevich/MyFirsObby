using System;
using UnityEngine;

[Serializable]
public class EnemySpawnerData
{
    public string Id;
    public MonsterTypeID MonsterTypeId;
    public Vector3 Position;

    public EnemySpawnerData(string id, MonsterTypeID monsterTypeId, Vector3 position)
    {
        Id = id;
        MonsterTypeId = monsterTypeId;
        Position = position;
    }
}