using System.Collections.Generic;
using UnityEngine;

public class WaypointChangerService : IWaypointChangerService
{
    public List<Waypoint> Waypoints { get; private set; }

    public WaypointChangerService() =>
        Waypoints = new List<Waypoint>();

    public void AddNewPoint(Waypoint waypoint) =>
        Waypoints.Add(waypoint);

    public Waypoint GetNextPoint()
    {
        int index = Random.Range(0, Waypoints.Count);
        return Waypoints[index];
    }

    public void Cleanup() =>
        Waypoints.Clear();
}