public interface IDifficultyService : IService
{
    void CheckUpGameMultiplier();
    void Initialize(LevelStaticData levelStaticData);

    void CharacterDeath();
}