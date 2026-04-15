using System.Collections.Generic;

public interface IWaypointChangerService : IService
{
    List<Waypoint> Waypoints { get; }

    void AddNewPoint(Waypoint waypoint);
    void Cleanup();
    Waypoint GetNextPoint();
}