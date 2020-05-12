using UnityEngine;

public static class AndroidUtils
{
    public static string GetFriendlyPath()
    {
        #if UNITY_ANDROID
            AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject applicationContext = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
            AndroidJavaObject path = applicationContext.Call<AndroidJavaObject>("getFilesDir");
            string filesPath = path.Call<string>("getCanonicalPath");
            return filesPath;
        #endif
            return "";
    }
    
}
