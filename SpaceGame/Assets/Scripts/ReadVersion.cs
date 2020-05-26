using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;

public class ReadVersion : MonoBehaviour
{
    public const  int MAJOR = 0;
    public const  int MINOR = 1;
    public static int PATCH { get; private set; }  = 0;

    public const string VERSION_DEPLOY_PATH = "deploy-version.txt";
    
    void Awake()
    {
      
        
        var str = GetComponent<TMP_Text>();
        
    #if UNITY_EDITOR
        var procInfo = new ProcessStartInfo("git", "rev-parse --abbrev-ref HEAD")
        {
            UseShellExecute = false, RedirectStandardOutput = true
        };
        
        var proc =  Process.Start(procInfo);
        str.text = proc?.StandardOutput.ReadLine();

        using (var stream = new StreamWriter(AndroidUtils.GetFriendlyPath() + VERSION_DEPLOY_PATH))
        {
            stream.WriteLine(str.text);
        }
    #else
        using (var stream = new StreamReader(AndroidUtils.GetFriendlyPath() + VERSION_DEPLOY_PATH))
        {
            str.text = stream.ReadLine();
        }
    #endif

    }
}
