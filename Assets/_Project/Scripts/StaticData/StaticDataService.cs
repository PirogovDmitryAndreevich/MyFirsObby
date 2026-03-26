using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticDataService : IStaticDataService
{
    private const string MonstersPath = "StaticData/Monsters";
    private const string LevelsPath = "StaticData/Levels";
    private const string WeaponsPath = "StaticData/Weapons";

    private Dictionary<MonsterTypeID, MonsterStaticData> _monsters;
    private Dictionary<string, LevelStaticData> _levels;
    private Dictionary<WeaponType, WeaponStaticData> _weapons;

    public void Load()
    {
        _monsters = Resources.LoadAll<MonsterStaticData>(MonstersPath)
            .ToDictionary(x => x.MonsterTypeID, x => x);

        _levels = Resources.LoadAll<LevelStaticData>(LevelsPath)
            .ToDictionary(x => x.LevelKey, x => x);

        _weapons = Resources.LoadAll<WeaponStaticData>(WeaponsPath)
            .ToDictionary(x => x.Type, x => x);
    }

    public MonsterStaticData ForMonster(MonsterTypeID typeId) =>
        _monsters.TryGetValue(typeId, out MonsterStaticData staticData)
        ? staticData
        : null;

    public LevelStaticData ForLevel(string sceneKey) =>
        _levels.TryGetValue(sceneKey, out LevelStaticData staticData)
        ? staticData
        : null;

    public Dictionary<WeaponType, WeaponStaticData> ForWeapons() =>
        _weapons;
}