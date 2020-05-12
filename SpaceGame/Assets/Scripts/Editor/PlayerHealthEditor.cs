using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerHealth))]
public class PlayerHealthEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Update View"))
        {
            (target as PlayerHealth)?.UpdateView();
        }
    }
}
