using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
public class MonsterStaticData : ScriptableObject
{
    public MonsterTypeId MonsterTypeID;

    public int Health;
    public float Damage;

    public int RewardScores;
    public int MinLoot;
    public int MaxLoot;

    public float MoveSpeed;

    public float EffectiveDistance;
    public float Cleavage;
    public float CooldownAttack;

    public GameObject Prefab;
}