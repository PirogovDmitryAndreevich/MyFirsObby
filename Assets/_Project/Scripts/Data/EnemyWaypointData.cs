using System;
using UnityEngine;

[Serializable]
public class EnemyWaypointData
{
    public string Id;
    public Vector3 Position;

    public EnemyWaypointData(string id, Vector3 position)
    {
        Id = id;
        Position = position;
    }
}