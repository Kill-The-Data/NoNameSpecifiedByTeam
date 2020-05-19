public static class AndroidUtils
{
    public static string GetFriendlyPath()
    {
        #if UNITY_ANDROID
        
            //get the path of the Unity-root directory on Android, so that we can actually write to disk
            AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject applicationContext = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
            AndroidJavaObject path = applicationContext.Call<AndroidJavaObject>("getFilesDir");
            string filesPath = path.Call<string>("getCanonicalPath");
            return filesPath;
        #endif
            //for all others, we should already be in the writeable root-directory
            return "";
    }
    
}
