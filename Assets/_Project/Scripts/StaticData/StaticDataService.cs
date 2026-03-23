using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticDataService : IStaticDataService
{
    private const string MonsterPath = "StaticData/Monsters";
    private const string LevelsPath = "StaticData/Levels";

    private Dictionary<MonsterTypeID, MonsterStaticData> _monsters;
    private Dictionary<string, LevelStaticData> _levels;

    public void Load()
    {
        _monsters = Resources.LoadAll<MonsterStaticData>(MonsterPath)
            .ToDictionary(x => x.MonsterTypeID, x => x);

        _levels = Resources.LoadAll<LevelStaticData>(LevelsPath)
            .ToDictionary(x => x.LevelKey, x => x);
    }

    public MonsterStaticData ForMonster(MonsterTypeID typeId) =>
        _monsters.TryGetValue(typeId, out MonsterStaticData staticData)
        ? staticData
        : null;

    public LevelStaticData ForLevel(string sceneKey) =>
        _levels.TryGetValue(sceneKey, out LevelStaticData staticData)
        ? staticData
        : null;
}