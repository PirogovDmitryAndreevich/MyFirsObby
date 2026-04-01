using System.Collections.Generic;

public interface IStaticDataService : IService
{
    CharacterView ForCharacterView(CharacterType type);
    public LevelStaticData ForLevel(string sceneKey);
    public MonsterStaticData ForMonster(MonsterTypeID typeId);
    public WeaponStaticData ForWeaponsData(WeaponType type);
    public ConveerStaticData ForConveer();
    public List<WeaponStaticData> AllWeapons();
    public void Load();
}