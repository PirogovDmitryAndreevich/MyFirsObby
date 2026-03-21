using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
public class MonsterStaticData : ScriptableObject
{
    public MonsterTypeID MonsterTypeID;

    public int Health;
    public float Damage;
    public float MoveSpeed;

    public GameObject Prefab;
    public float EffectiveDistance;
    public float Cleavage;
    public float CooldownAttack;
}