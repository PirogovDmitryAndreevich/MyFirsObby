using System.Collections.Generic;

public interface IStaticDataService : IService
{
    public LevelStaticData ForLevel(string sceneKey);
    public MonsterStaticData ForMonster(MonsterTypeID typeId);
    public Dictionary<WeaponType, WeaponStaticData> ForWeapons();
    public void Load();
}