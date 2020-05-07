#if(UNITY_EDITOR)

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HideMouse))]
public class HideMouseEditor : Editor
{
    public override void OnInspectorGUI()
    {

        var hidemouse = target as HideMouse;
        hidemouse.AutoHide = GUILayout.Toggle(hidemouse.AutoHide, "Auto Hide");


        if (GUILayout.Button("Hide"))
            Cursor.visible = false;
        if (GUILayout.Button("Show"))
            Cursor.visible = true;
    }
}

#endif