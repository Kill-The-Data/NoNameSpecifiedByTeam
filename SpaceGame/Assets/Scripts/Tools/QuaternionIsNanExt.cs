﻿using UnityEngine;

public static class QuaternionIsNanExt
{
    public static bool IsNaN(this Quaternion q) {
        return float.IsNaN(q.x) || float.IsNaN(q.y) || float.IsNaN(q.z) || float.IsNaN(q.w);
    }
}

public static class Vector3IsNanExt
{
    public static bool IsNan(this Vector3 v)
    {
        return float.IsNaN(v.x) || float.IsNaN(v.y) || float.IsNaN(v.z);
    }
}

public static class Vector2Promote
{
    public static Vector3 Promote(this Vector2 v)
    {
        return new Vector3(v.x,v.y);
    }
}