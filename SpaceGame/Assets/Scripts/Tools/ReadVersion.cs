using System.Collections;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
[RequireComponent(typeof(TMP_Text))]
public class ReadVersion : MonoBehaviour
{
    public const int MAJOR = 0;
    public const int MINOR = 1;
    public static int PATCH { get; private set; } = 0;

    public const string WEBGL_ROOT = "https://kill-the-data.github.io/NoNameSpecifiedByTeam/";
    public const string VERSION_DEPLOY_PATH = "deploy-version.txt";

    void Awake()
    {


        var str = GetComponent<TMP_Text>();

#if UNITY_EDITOR

        string commitSHA = CallApplication("git", "rev-parse HEAD").Substring(0, 10);
        string commitName = CallApplication("git", "rev-parse --abbrev-ref HEAD");

        str.text = $"{commitName}:  {commitSHA}";

        using (var stream = new StreamWriter(AndroidUtils.GetFriendlyPath() + VERSION_DEPLOY_PATH))
        {
            stream.WriteLine(str.text);
        }

        CallApplication("copy-deploy-string.bat", "-find-me");

#else
#if UNITY_WEBGL
            StartCoroutine(SetTextAfterDownload(WEBGL_ROOT+VERSION_DEPLOY_PATH, str));
#else
            using (var stream = new StreamReader(AndroidUtils.GetFriendlyPath() + VERSION_DEPLOY_PATH))
            {
                str.text = stream.ReadLine();
            }
#endif
#endif

    }
    private IEnumerator SetTextAfterDownload(string path, TMP_Text str)
    {
        using (var www = UnityWebRequest.Get(path))
        {
            www.timeout = 1;
            yield return www.SendWebRequest();
            str.text = www.downloadHandler.text;
        }
    }

    private string CallApplication(string path, string args)
    {
        var procInfo = new ProcessStartInfo(path, args)
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };

        var proc = Process.Start(procInfo);
        return proc?.StandardOutput.ReadLine();
    }

}
