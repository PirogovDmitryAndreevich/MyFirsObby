public interface IStaticDataService : IService
{
    public LevelStaticData ForLevel(string sceneKey);
    public MonsterStaticData ForMonster(MonsterTypeID typeId);
    public void Load();
}