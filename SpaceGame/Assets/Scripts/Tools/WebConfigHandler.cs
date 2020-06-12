using System;
using System.Collections;
using System.Threading;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;


public static class WebConfigHandler
{
    
    //where to get the data from
    private const string CONFIG_URI_LATEST = "https://kill-the-data.github.io/config/spacegame/latest.json";
    
    private static JObject m_deserializedData;

    private static bool m_downloadFinished = false;
    private static Action<JObject> m_OnFinishedAction = delegate { };
    
    public static IEnumerator FetchConfig()
    {
        for(int i = 0; i < 4 ; ++i)
        {
            m_downloadFinished = false;
            //make http request
            
            var www = UnityWebRequest.Get(CONFIG_URI_LATEST);
            
            //apparently unity wants to read the www stream until the universe has ended, luckily we 
            //can tell unity that the universe ends in exactly 8 seconds ( 4 * 2 )
            www.timeout = 2;
            yield return www.SendWebRequest();
            
            //get the response text from the request
            string response = www.downloadHandler.text;
            
            //clean the response (apparently "real" json is not supposed to have newlines in it ... ew
            response = response.Replace("\n","");
            response = response.Replace("\r","");
            
            if(!String.IsNullOrEmpty(response)) {
            
                //parse the json
                m_deserializedData = JObject.Parse(response);
                m_downloadFinished = true;
                m_OnFinishedAction(m_deserializedData);
                yield break;
            }
        }
    }

    public static JObject GetConfig()
    {
        for (int i = 0; i < 10 && m_deserializedData == null; ++i)
        {
            Thread.Sleep(100);
        }
        if(m_deserializedData == null)
            Debug.LogWarning("Gave up waiting for www");
        
        return m_deserializedData;
    }

    public static void OnFinishDownload(Action<JObject> action)
    {
        if (m_downloadFinished)
        {
            action(m_deserializedData);
        }
        m_OnFinishedAction += action;
    }
    
    
    public static JObject UncheckedGetConfig()
    {
        return m_deserializedData;
    }

    public static void ExtractInt(this JObject j,string key, Action<int> surrogate)
    {
        if (int.TryParse(j[key].ToString(), out int value))
        {
            surrogate(value);
        }
    }
    
}
