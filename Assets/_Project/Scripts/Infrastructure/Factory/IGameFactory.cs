using UnityEngine;

public interface IGameFactory : IService
{
    CharacterController CreateCharacterController(Transform at);
    ArsenalView CreateArsenal(Transform at, IInventory inventory, IIndicateUI indicator);
    GameObject CreateArsenalShop();
    GameObject CreateConveer(GameObject at);
    void CreateConveerManager(Conveer conveerComponent);
    Hud CreateHod(bool isMobile);
    Inventory CreateInventory(CharacterController character);
    LootPiece CreateLoot();
    void CreateMonster(MonsterTypeID monsterTypeID, Transform parent);
    void CreateSpawner(Vector3 at, string id, MonsterTypeID monsterTypeId);
}