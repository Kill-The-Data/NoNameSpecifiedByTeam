
using UnityEditor;
using UnityEngine;

public class ConfigHelper : EditorWindow
{
    [MenuItem("Window/ConfigHelper")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<ConfigHelper>();
    }

    private void OnGUI()
    {
        //fetch data and check if we can query a field
        if (GUILayout.Button("Test Fetch"))
        {
            var conf = WebConfigHandler.GetConfig();
            Debug.Log(conf["health"]);
        }
    }
}
