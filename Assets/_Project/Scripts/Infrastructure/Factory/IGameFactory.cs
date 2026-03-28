using UnityEngine;

public interface IGameFactory : IService
{
    GameObject CreateArsenal(GameObject at);
    GameObject CreateArsenalShop();
    GameObject CreateCharacter(GameObject at);
    GameObject CreateConveer(GameObject at);
    void CreateConveerManager(Conveer conveerComponent);
    GameObject CreateHod(bool isMobile);
    Inventory CreateInventory();
    LootPiece CreateLoot();
    void CreateMonster(MonsterTypeID monsterTypeID, Transform parent);
    void CreateSpawner(Vector3 at, string id, MonsterTypeID monsterTypeId);
}