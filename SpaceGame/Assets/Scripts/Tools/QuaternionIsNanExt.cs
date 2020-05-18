using UnityEngine;

public static class QuaternionIsNanExt
{
    public static bool IsNaN(this Quaternion q) {
        return float.IsNaN(q.x) || float.IsNaN(q.y) || float.IsNaN(q.z) || float.IsNaN(q.w);
    }
}
