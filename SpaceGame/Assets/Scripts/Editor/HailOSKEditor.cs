#if(UNITY_EDITOR)

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HailOSK))]
public class HailOSKEditor : Editor
{
    public override void OnInspectorGUI()
    {
        
        //get default inspector gui
        base.OnInspectorGUI();
        
        var osk = target as HailOSK;
        if(osk == null) Debug.LogError("target was not of type HailOSK how in gods name is that even possible");
        else {
            
            //show a button to hail the on-screen-keyboard
            if (GUILayout.Button("Hail Keyboard"))
            {
                osk.Hail();
            }
        }
    }
}
#endif