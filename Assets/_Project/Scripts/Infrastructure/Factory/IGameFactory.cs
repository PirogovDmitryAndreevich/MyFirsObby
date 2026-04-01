using UnityEngine;

public interface IGameFactory : IService
{
    Character CreateCharacterController(Transform at);
    ArsenalView CreateArsenal(Transform at, IIndicateUI indicator, IWindow arsenalWindow);
    GameObject CreateConveer(GameObject at);
    void CreateConveerManager(Conveer conveerComponent);
    WeaponSelector CreateWeaponSelector(Character character);
    LootPiece CreateLoot();
    void CreateMonster(MonsterTypeID monsterTypeID, Transform parent);
    void CreateSpawner(Vector3 at, string id, MonsterTypeID monsterTypeId);
}