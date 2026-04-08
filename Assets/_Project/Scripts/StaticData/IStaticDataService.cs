using System.Collections.Generic;

public interface IStaticDataService : IService
{
    CharacterView ForCharacterView(CharacterType type);
    public LevelStaticData ForLevel(string sceneKey);
    public MonsterStaticData ForMonster(MonsterTypeId typeId);
    public WeaponStaticData ForWeaponsData(WeaponType type);
    public ConveerStaticInfo ForConveers(ConveerNumbers number);
    public List<WeaponStaticData> AllWeapons();
    public List<ConveerStaticInfo> AllConveers();
    public void Load();
}