using System;
using System.Collections;
using System.Threading;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;


public static class WebConfigHandler
{
    
    //where to get the data from
    private const string CONFIG_URI_SPECIFIC = "https://kill-the-data.github.io/config/spacegame/v{0}.json";
    
    //the version of the config
    private const int CONFIG_VERSION = 2;
    
    private static JObject m_deserializedData;
    public static IEnumerator FetchConfig()
    {
        //make http request

        string request_uri = String.Format(CONFIG_URI_SPECIFIC, CONFIG_VERSION);
        
        var www = UnityWebRequest.Get(request_uri);
        
        //apparently unity want to read the www stream until the universe has ended, luckily we 
        //can tell unity that the universe ends in exactly one second
        www.timeout = 1;
        yield return www.SendWebRequest();
        
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
        for (int i = 0; i < 10 && m_deserializedData == null; ++i)
        {
            Thread.Sleep(100);
        }
        if(m_deserializedData == null)
            Debug.LogWarning("Gave up waiting for www");
        
        return m_deserializedData;
    }
    
    public static JObject UncheckedGetConfig()
    {
        return m_deserializedData;
    }
}
