using Cinemachine;
using UnityEngine;

public interface IGameFactory : IService
{
    CinemachineFreeLook CreateCamera();
    Build CreateBuild(Transform at, LevelStaticData levelStaticData);
    ATM CreateATM(Transform at);
    Bullet CreateBullet();
    Character CreateCharacterController(Transform at);
    ArsenalInitializer CreateArsenal(Transform at, LevelStaticData levelData, IIndicateUI indicator, IWindow arsenalWindow);
    ConveerInitializer CreateConveerView(Transform at, ATM atm, IIndicateUI indicator, IWindow conveerShopWindow);
    WeaponSelector CreateWeaponSelector(Character character);
    Weapon CreateWeapon(WeaponProgress weaponProgress);
    LootPiece CreateLoot();
    Enemy CreateMonster(MonsterTypeId monsterTypeID, Transform parent);
    SpawnPoint CreateSpawner(Vector3 at, string id, MonsterTypeId monsterTypeId);
    public Waypoint CreateWaypoint(Vector3 at);
}