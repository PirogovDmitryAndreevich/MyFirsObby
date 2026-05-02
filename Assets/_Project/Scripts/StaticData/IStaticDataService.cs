using System.Collections.Generic;

public interface IStaticDataService : IService
{
    public CharacterView ForCharacterView(CharacterType type);
    public LevelStaticData ForLevel(string sceneKey);
    public MonsterStaticData ForMonster(MonsterTypeId typeId);
    public WeaponStaticData ForWeaponsData(WeaponType type);
    public List<WeaponStaticData> AllWeapons();
    public void Load();
    public CharacterStartData GetCharacterStartData();
    public BuildStaticData ForBuild();
    public ConveyerStaticData ForConveyer(ConveyerType type);
    public ForOpener ForOpeners(OpeningType type);
}