using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticDataService : IStaticDataService
{
    private const string MonstersPath = "StaticData/Monsters";
    private const string LevelsPath = "StaticData/Levels";
    private const string WeaponsPath = "StaticData/Weapons";
    private const string CharactersViewPath = "StaticData/CharactersView/Characters";
    private const string CharacterDataPath = "StaticData/CharacterData/CharacterData";
    private const string BuildDataPath = "StaticData/BuildData/BuildData";

    private CharacterStartData _characterStartData;
    private BuildStaticData _buildData;
    private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;
    private Dictionary<string, LevelStaticData> _levels;
    private Dictionary<WeaponType, WeaponStaticData> _weaponsData;
    private Dictionary<CharacterType, CharacterView> _charactersView;
    private Dictionary<ConveyerType, ConveyerStaticData> _conveyers;
    private Dictionary<OpeningType, ForOpener> _forOpeners;

    public void Load()
    {
        _monsters = Resources.LoadAll<MonsterStaticData>(MonstersPath)
            .ToDictionary(x => x.MonsterTypeID, x => x);

        _levels = Resources.LoadAll<LevelStaticData>(LevelsPath)
            .ToDictionary(x => x.LevelKey, x => x);

        _weaponsData = Resources.LoadAll<WeaponStaticData>(WeaponsPath)
            .ToDictionary(x => x.Type, x => x);

        _charactersView = Resources.Load<CharacterViewStaticData>(CharactersViewPath)
            .Characters.ToDictionary(x=>x.Type, x => x);

        _characterStartData = Resources.Load<CharacterStartData>(CharacterDataPath);

        _buildData = Resources.Load<BuildStaticData>(BuildDataPath);

        _conveyers = _buildData.Conveyers.ToDictionary(x => x.Type, x => x);

        _forOpeners = _buildData.ForOpeners.ToDictionary(x => x.Type, x => x);
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

    public CharacterView ForCharacterView(CharacterType type) =>
        _charactersView.TryGetValue(type, out CharacterView view)
        ? view 
        : null;

    public List<WeaponStaticData> AllWeapons() =>
        _weaponsData.Values.ToList();

    public CharacterStartData GetCharacterStartData() =>
        _characterStartData;

    public BuildStaticData ForBuild() =>
        _buildData;

    public ConveyerStaticData ForConveyer(ConveyerType type) =>
        _conveyers.TryGetValue(type, out ConveyerStaticData data)
        ? data
        : null;

    public ForOpener ForOpeners(OpeningType type) =>
        _forOpeners.TryGetValue(type, out ForOpener data)
        ? data
        : null;
}