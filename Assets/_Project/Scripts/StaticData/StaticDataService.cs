using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticDataService : IStaticDataService
{
    private const string MonstersPath = "StaticData/Monsters";
    private const string LevelsPath = "StaticData/Levels";
    private const string WeaponsPath = "StaticData/Weapons";
    private const string ConveerPath = "StaticData/Conveer/ConveerData";
    private const string CharactersPath = "StaticData/CharactersView/Characters";

    private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;
    private Dictionary<string, LevelStaticData> _levels;
    private Dictionary<WeaponType, WeaponStaticData> _weaponsData;
    private Dictionary<CharacterType, CharacterView> _charactersView;
    private Dictionary<ConveerNumbers, ConveerStaticInfo> Conveers;

    public void Load()
    {
        _monsters = Resources.LoadAll<MonsterStaticData>(MonstersPath)
            .ToDictionary(x => x.MonsterTypeID, x => x);

        _levels = Resources.LoadAll<LevelStaticData>(LevelsPath)
            .ToDictionary(x => x.LevelKey, x => x);

        _weaponsData = Resources.LoadAll<WeaponStaticData>(WeaponsPath)
            .ToDictionary(x => x.Type, x => x);

        Conveers = Resources.Load<ConveerStaticData>(ConveerPath)
            .StaticInfo.ToDictionary(x => x.Number, x => x);

        _charactersView = Resources.Load<CharacterViewStaticData>(CharactersPath)
            .Characters.ToDictionary(x=>x.Type, x => x);
    }

    public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
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

    public ConveerStaticInfo ForConveers(ConveerNumbers number) =>
        Conveers.TryGetValue(number, out ConveerStaticInfo info)
        ? info 
        : null;

    public CharacterView ForCharacterView(CharacterType type) =>
        _charactersView.TryGetValue(type, out CharacterView view)
        ? view 
        : null;

    public List<WeaponStaticData> AllWeapons() =>
        _weaponsData.Values.ToList();

    public List<ConveerStaticInfo> AllConveers() =>
        Conveers.Values.ToList();
}