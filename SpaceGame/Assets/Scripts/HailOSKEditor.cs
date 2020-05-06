#if(UNITY_EDITOR)


using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HailOSK))]
public class HailOSKEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var osk = target as HailOSK;
        if(osk == null) Debug.LogError("target was not of type HailOSK how in gods name is that even possible");
        else {
            osk.method = (HailOSK.Method)EditorGUILayout.EnumFlagsField("Method", osk.method) ;

            if (GUILayout.Button("Hail Keyboard"))
            {
                osk.Hail();
            }
        }
    }
}

#endif