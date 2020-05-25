
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

public class ConfigHelper : EditorWindow
{
    [MenuItem("Window/ConfigHelper")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<ConfigHelper>();
    }

    public void ReceivedData(JObject conf)
    {
        Debug.Log(conf?["health"]);
    }

    private void OnGUI()
    {
        //fetch data and check if we can query a field
        if (GUILayout.Button("Test Fetch"))
        {
            var x = WebConfigHandler.FetchConfig();
            WebConfigHandler.OnFinishDownload(ReceivedData);
        }
    }
}
