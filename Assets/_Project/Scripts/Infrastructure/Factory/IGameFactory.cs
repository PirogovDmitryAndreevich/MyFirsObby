using Cinemachine;
using UnityEngine;

public interface IGameFactory : IService
{
    CinemachineFreeLook CreateCamera();
    Build CreateBuild(Transform at, LevelStaticData levelStaticData, RewardAdvPopup rewardPopup,
        IIndicateUI indicator, IWindow arsenalWindow);
    Bullet CreateBullet();
    Character CreateCharacterController(Vector3 at);
    WeaponSelector CreateWeaponSelector(Character character);
    Weapon CreateWeapon(WeaponProgress weaponProgress);
    LootPiece CreateLoot();
    Enemy CreateMonster(MonsterTypeId monsterTypeID);
    SpawnPoint CreateSpawner(Vector3 at, string id, MonsterTypeId monsterTypeId);
    void CreateLeaderBoard(Transform at);
    Waypoint CreateWaypoint(Vector3 at);
}