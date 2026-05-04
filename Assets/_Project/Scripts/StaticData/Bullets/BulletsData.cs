using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletsData", menuName = "StaticData/Bullets")]
public class BulletsData : ScriptableObject
{
    public List<Bullet> Bullets;
}