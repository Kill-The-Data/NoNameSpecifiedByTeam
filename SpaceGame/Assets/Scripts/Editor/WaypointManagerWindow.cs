using System;
using UnityEditor;
using UnityEngine;

public class WaypointManagerWindow : EditorWindow
{
    [MenuItem("Tools/Waypoint Editor")]
    public static void Show()
    {
        GetWindow<WaypointManagerWindow>();
    }

    public Transform WaypointRoot;

    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);

        EditorGUILayout.PropertyField(obj.FindProperty("WaypointRoot"));

        if (WaypointRoot == null)
        {
            EditorGUILayout.HelpBox("Root transform must be selected",MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("box");

            DrawButtons();
            
            EditorGUILayout.EndVertical();
        }

        obj.ApplyModifiedProperties();
    }

    private void DrawButtons()
    {
        if (GUILayout.Button("Create Waypoint"))
        {
            CreateWaypoint();
        }

        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponentSafe(out Waypoint waypoint))
        {
            if (GUILayout.Button("Create Waypoint Before"))
            {
                CreateWaypointBefore(waypoint);
            }
            if (GUILayout.Button("Create Waypoint After"))
            {
                CreateWaypointAfter(waypoint);
            }
            if (GUILayout.Button("Remove Waypoint"))
            {
                RemoveWaypoint(waypoint);
            }
        }
        
    }

    private void CreateWaypoint()
    {
        GameObject waypointObject = 
            new GameObject("Waypoint" + WaypointRoot.childCount,typeof(Waypoint));
        waypointObject.transform.SetParent(WaypointRoot,false);

        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();

        if (WaypointRoot.childCount > 1)
        {
            waypoint.PreviousWaypoint =
                WaypointRoot.GetChild(WaypointRoot.childCount - 2).GetComponent<Waypoint>();

            waypoint.PreviousWaypoint.NextWaypoint = waypoint;
            
            var tf = waypoint.PreviousWaypoint.transform;
            waypoint.transform.position = tf.position;
            waypoint.transform.rotation = tf.rotation;
        }

        Selection.activeGameObject = waypointObject;
    }

    private void CreateWaypointBefore(Waypoint waypoint)
    {
        GameObject waypointObject = new GameObject("Waypoint" + WaypointRoot.childCount,typeof(Waypoint));
        waypointObject.transform.SetParent(WaypointRoot,false);

        var newWaypoint = waypointObject.GetComponent<Waypoint>();

        waypointObject.transform.position = waypoint.transform.position;
        waypointObject.transform.rotation = waypoint.transform.rotation;

        if (waypoint.PreviousWaypoint != null)
        {
            newWaypoint.PreviousWaypoint = waypoint.PreviousWaypoint;
            waypoint.PreviousWaypoint.NextWaypoint = newWaypoint;
        }

        newWaypoint.NextWaypoint = waypoint;
        waypoint.PreviousWaypoint = newWaypoint;
        
        newWaypoint.transform.SetSiblingIndex(waypoint.transform.GetSiblingIndex());

        Selection.activeGameObject = waypointObject;
    }

    private void CreateWaypointAfter(Waypoint waypoint)
    {
        GameObject waypointObject = new GameObject("Waypoint" + WaypointRoot.childCount,typeof(Waypoint));
        waypointObject.transform.SetParent(WaypointRoot,false);
        var newWaypoint = waypointObject.GetComponent<Waypoint>();

        waypointObject.transform.position = waypoint.transform.position;
        waypointObject.transform.rotation = waypoint.transform.rotation;

        newWaypoint.PreviousWaypoint = waypoint;

        if (waypoint.NextWaypoint != null)
        {
            waypoint.NextWaypoint.PreviousWaypoint = newWaypoint;
            newWaypoint.NextWaypoint = waypoint.NextWaypoint;
        }

        waypoint.NextWaypoint = newWaypoint;
        
        newWaypoint.transform.SetSiblingIndex(waypoint.transform.GetSiblingIndex());


    }

    private void RemoveWaypoint(Waypoint waypoint)
    {
        if (waypoint.NextWaypoint != null)
        {
            waypoint.NextWaypoint.PreviousWaypoint = waypoint.PreviousWaypoint;
            Selection.activeGameObject = waypoint.NextWaypoint.gameObject;

        }

        if (waypoint.PreviousWaypoint != null)
        {
            waypoint.PreviousWaypoint.NextWaypoint = waypoint.NextWaypoint;
            Selection.activeGameObject = waypoint.PreviousWaypoint.gameObject;
        }
        
        DestroyImmediate(waypoint.gameObject);
    }
    
}
