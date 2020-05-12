#if(UNITY_EDITOR)

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HideMouse))]
public class HideMouseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //add checkbox for AutoHiding the mouse
        var hideMouse = target as HideMouse;
        if (hideMouse != null) hideMouse.AutoHide = GUILayout.Toggle(hideMouse.AutoHide, "Auto Hide");

        //also add some utility buttons to hide or show the mouse manually
        if (GUILayout.Button("Hide"))
            Cursor.visible = false;
        if (GUILayout.Button("Show"))
            Cursor.visible = true;
    }
}

#endif