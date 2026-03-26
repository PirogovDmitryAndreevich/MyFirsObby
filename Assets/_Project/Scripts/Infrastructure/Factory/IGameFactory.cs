using UnityEngine;

public interface IGameFactory : IService
{
    void CreateArsenal(GameObject at);
    GameObject CreateCharacter(GameObject at);
    GameObject CreateHod(bool isMobile);
    Inventory CreateInventory();
    LootPiece CreateLoot();
    void CreateMonster(MonsterTypeID monsterTypeID, Transform parent);
    void CreateSpawner(Vector3 at, string id, MonsterTypeID monsterTypeId);
}