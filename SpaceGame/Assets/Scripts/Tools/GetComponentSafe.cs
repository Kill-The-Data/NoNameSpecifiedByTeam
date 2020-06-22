using UnityEngine;


public static class GetComponentSafeExt
{
    public static bool GetComponentSafe<T>(this GameObject behaviour, out T component) where T : Component
    { 
        var c = behaviour.GetComponent<T>();
        if (c != null && c)
        {
            component = c;
            return true;
        }
        else
        {
            component = null;
            return false;
        }
    }
    public static bool GetComponentSafe<T>(this MonoBehaviour behaviour, out T component) where T : Component
    {
        var c = behaviour.GetComponent<T>();
        if (c != null && c)
        {
            component = c;
            return true;
        }
        else
        {
            component = null;
            return false;
        }
    }
    public static bool GetComponentSafe<T>(this Transform behaviour, out T component) where T : Component
    {
        var c = behaviour.GetComponent<T>();
        if (c != null && c)
        {
            component = c;
            return true;
        }
        else
        {
            component = null;
            return false;
        }
    }
    public static bool GetComponentSafe<T>(this Collider behaviour, out T component) where T : Component
    {
        var c = behaviour.GetComponent<T>();
        if (c != null && c)
        {
            component = c;
            return true;
        }
        else
        {
            component = null;
            return false;
        }
    }
}
