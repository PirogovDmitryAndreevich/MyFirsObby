public interface IStaticDataService : IService
{
    public MonsterStaticData ForMonster(MonsterTypeID typeId);
    public void LoadMonsters();
}