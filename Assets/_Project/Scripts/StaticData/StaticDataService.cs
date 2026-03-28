using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class StaticDataService : IStaticDataService
{
    private const string MonstersPath = "StaticData/Monsters";
    private const string LevelsPath = "StaticData/Levels";
    private const string WeaponsPath = "StaticData/Weapons";
    private const string ConveerPath = "StaticData/Conveer/ConveerData";
    private const string CharactersPath = "StaticData/CharactersView/Characters";

    private ConveerStaticData _conveers;
    private Dictionary<MonsterTypeID, MonsterStaticData> _monsters;
    private Dictionary<string, LevelStaticData> _levels;
    private Dictionary<WeaponType, WeaponStaticData> _weaponsData;
    private Dictionary<CharacterType, CharacterView> _charactersView;

    public void Load()
    {
        _monsters = Resources.LoadAll<MonsterStaticData>(MonstersPath)
            .ToDictionary(x => x.MonsterTypeID, x => x);

        _levels = Resources.LoadAll<LevelStaticData>(LevelsPath)
            .ToDictionary(x => x.LevelKey, x => x);

        _weaponsData = Resources.LoadAll<WeaponStaticData>(WeaponsPath)
            .ToDictionary(x => x.Type, x => x);

        _conveers = Resources.Load<ConveerStaticData>(ConveerPath);

        _charactersView = Resources.Load<CharacterViewStaticData>(CharactersPath)
            .Characters.ToDictionary(x=>x.Type, x => x);
    }

    public MonsterStaticData ForMonster(MonsterTypeID typeId) =>
        _monsters.TryGetValue(typeId, out MonsterStaticData staticData)
        ? staticData
        : null;

    public LevelStaticData ForLevel(string sceneKey) =>
        _levels.TryGetValue(sceneKey, out LevelStaticData staticData)
        ? staticData
        : null;

    public WeaponStaticData ForWeaponsData(WeaponType type) =>
        _weaponsData.TryGetValue(type, out WeaponStaticData weaponData)
        ? weaponData
        : null;

    public ConveerStaticData ForConveer() =>
        _conveers;

    public CharacterView ForCharacterView(CharacterType type) =>
        _charactersView.TryGetValue(type, out CharacterView view)
        ? view 
        : null;
}