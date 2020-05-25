using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerHealth))]
public class PlayerHealthEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //get default inspector gui
        base.OnInspectorGUI();
        
        //as the data in the Editor is adjusted only on the standard 
        //[SerializeField] method we also need something to updated the view
        //manually as that will only happen on Property access
        if (GUILayout.Button("Update View"))
        {
            (target as PlayerHealth)?.UpdateView();
        }

        if (GUILayout.Button("Die Already"))
        {
            (target as PlayerHealth)?.TakeDamage(100000);
        }
    }
}
