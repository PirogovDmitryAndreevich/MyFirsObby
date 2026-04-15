using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaypointMarker))]
public class WaypointMarkerEditor : Editor
{
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(WaypointMarker marker, GizmoType gizmo)
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(marker.transform.position, 0.5f);
    }
}