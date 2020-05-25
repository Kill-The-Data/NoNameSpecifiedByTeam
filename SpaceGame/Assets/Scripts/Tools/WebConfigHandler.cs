using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;


public class UnityWebRequestAwaiter : INotifyCompletion
{
    private UnityWebRequestAsyncOperation asyncOp;
    private Action continuation;

    public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOp)
    {
        this.asyncOp = asyncOp;
        asyncOp.completed += OnRequestCompleted;
    }

    public bool IsCompleted { get { return asyncOp.isDone; } }

    public void GetResult() { }

    public void OnCompleted(Action continuation)
    {
        this.continuation = continuation;
    }

    private void OnRequestCompleted(AsyncOperation obj)
    {
        continuation?.Invoke();
    }
}

public static class ExtensionMethods
{
    public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
    {
        return new UnityWebRequestAwaiter(asyncOp);
    }
}

public static class WebConfigHandler
{
    
    //where to get the data from
    private const string CONFIG_URI_SPECIFIC = "https://kill-the-data.github.io/config/spacegame/v{0}.json";
    
    //the version of the config
    private const int CONFIG_VERSION = 2;
    
    private static JObject m_deserializedData;
    public static async void FetchConfig()
    {
        //make http request
        var www = UnityWebRequest.Get(String.Format(CONFIG_URI_SPECIFIC,CONFIG_VERSION));
        await www.SendWebRequest();
        
        //get the response text from the request
        string response = www.downloadHandler.text;
        
        //clean the response (apparently "real" json is not supposed to have newlines in it ... ew
        response = response.Replace("\n","");
        response = response.Replace("\r","");
        
        //parse the json
        m_deserializedData = JObject.Parse(response);
    }

    public static JObject GetConfig()
    {
        if(m_deserializedData == null)
            FetchConfig();
        return m_deserializedData;
    }
}
