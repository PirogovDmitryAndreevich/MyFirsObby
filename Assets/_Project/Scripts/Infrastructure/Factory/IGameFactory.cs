using UnityEngine;

public interface IGameFactory : IService
{
    GameObject CreateCharacter(GameObject at);
    GameObject CreateHod(bool isMobile);
    LootPiece CreateLoot();
    void CreateMonster(MonsterTypeID monsterTypeID, Transform parent);
    void CreateSpawner(Vector3 at, string id, MonsterTypeID monsterTypeId);
}