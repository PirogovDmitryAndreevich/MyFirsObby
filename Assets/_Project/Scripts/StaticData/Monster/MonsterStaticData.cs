using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
public class MonsterStaticData : ScriptableObject
{
    public MonsterTypeID MonsterTypeID;

    public int Health;
    public float Damage;

    public int MinLoot;
    public int MixLoot;

    public float MoveSpeed;

    public float EffectiveDistance;
    public float Cleavage;
    public float CooldownAttack;

    public GameObject Prefab;
}