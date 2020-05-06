using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HailOSK : MonoBehaviour
{
    public enum Method
    {
        TabTip,
        OSK
    }

    [Header(" --- Setup ---")]

    [Tooltip("The keyboard to hail")]
    public Method method = Method.TabTip;

    public void Hail()
    {
        //hail the on screen keyboard, for windows 10 tabtip is the new method
        System.Diagnostics.Process.Start(method == Method.TabTip ? "tabtip.exe" : "osk.exe");
    }
}
