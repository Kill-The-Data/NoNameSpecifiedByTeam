using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HailOSK))]
public class HailOSKEditor : Editor
{
    private HailOSK.Method m_method;

    public override void OnInspectorGUI()
    {



        (target as HailOSK).method = (HailOSK.Method)EditorGUILayout.EnumFlagsField("Method", (target as HailOSK).method) ;

        if (GUILayout.Button("Hail Keyboard"))
        {
            (target as HailOSK)?.Hail();
        }
    }
}
