using Cinemachine;
using UnityEngine;

public interface IGameFactory : IService
{
    CinemachineFreeLook CreateCamera();
    Build CreateBuild(Transform at, LevelStaticData levelStaticData, RewardAdvPopup rewardPopup, IWeaponContainer container);
    Character CreateCharacterController(Vector3 at);
    WeaponSelector CreateWeaponSelector(Character character, IWeaponContainer container);
    WeaponContainer CreateWeaponContainer();
    WeaponBase CreateWeapon(WeaponType type, Transform parent);
    LootPiece CreateLoot();
    Enemy CreateMonster(MonsterTypeId monsterTypeID);
    SpawnPoint CreateSpawner(Vector3 at, string id, MonsterTypeId monsterTypeId);
    void CreateLeaderBoard(Transform at);
    Waypoint CreateWaypoint(Vector3 at);
    DamageImpactPiece CreateDamageImpact();
    DeathEffect CreateEnemyDeathEffect();
}