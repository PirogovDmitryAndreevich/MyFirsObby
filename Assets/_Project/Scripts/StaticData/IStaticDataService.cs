public interface IStaticDataService : IService
{
    CharacterView ForCharacterView(CharacterType type);
    public LevelStaticData ForLevel(string sceneKey);
    public MonsterStaticData ForMonster(MonsterTypeID typeId);
    public WeaponStaticData ForWeaponsData(WeaponType type);
    public ConveerStaticData ForConveer();
    public void Load();
}