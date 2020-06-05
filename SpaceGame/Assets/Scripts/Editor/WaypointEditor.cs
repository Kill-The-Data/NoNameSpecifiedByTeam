using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class WaypointEditor 
{
   [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
   public static void OnDrawSceneGizmo(Waypoint waypoint, GizmoType gizmoType)
   {
      if ((gizmoType & GizmoType.Selected) != 0)
      {
         Gizmos.color = Color.yellow;
      }
      else
      {
         Gizmos.color = Color.yellow * 0.5f;
      }
      
      var transform = waypoint.transform;
      var position = transform.position;
      var right = transform.right;

      Gizmos.DrawSphere(position, .5f);
      Gizmos.DrawLine(  position + (right * waypoint.Width /2f),
                        position - (right * waypoint.Width /2f));

      if (waypoint.PreviousWaypoint != null)
      {
         Gizmos.color = Color.red;

         var prevWaypointTransform = waypoint.PreviousWaypoint.transform;

         Vector3 offset = right * waypoint.Width / 2f;
         Vector3 offsetTo = prevWaypointTransform.right * waypoint.PreviousWaypoint.Width / 2;
         
         Gizmos.DrawLine(waypoint.transform.position + offset, prevWaypointTransform.position + offsetTo);
         
      }
      if (waypoint.NextWaypoint != null)
      {
         Gizmos.color = Color.green;

         var nextWaypointTransform = waypoint.NextWaypoint.transform;

         Vector3 offset = -right * waypoint.Width / 2f;
         Vector3 offsetTo = -nextWaypointTransform.right * waypoint.NextWaypoint.Width / 2;
         
         Gizmos.DrawLine(waypoint.transform.position + offset, nextWaypointTransform.position + offsetTo);
         
      }
      
   }
}
