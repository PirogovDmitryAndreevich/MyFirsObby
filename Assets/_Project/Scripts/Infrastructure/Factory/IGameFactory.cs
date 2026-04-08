using UnityEngine;

public interface IGameFactory : IService
{
    Character CreateCharacterController(Transform at);
    ArsenalInitializer CreateArsenal(Transform at, IIndicateUI indicator, IWindow arsenalWindow);
    ConveerInitializer CreateConveerView(GameObject at, IIndicateUI indicator, IWindow conveerShopWindow);
    WeaponSelector CreateWeaponSelector(Character character);
    LootPiece CreateLoot();
    void CreateMonster(MonsterTypeId monsterTypeID, Transform parent);
    void CreateSpawner(Vector3 at, string id, MonsterTypeId monsterTypeId);
}