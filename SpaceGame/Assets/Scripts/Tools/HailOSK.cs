using System;
using System.Runtime.InteropServices;
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
        if (method == Method.TabTip)
        {
            var uiHostNoLaunch = new UIHostNoLaunch();
            var tipInvocationElement = uiHostNoLaunch as ITipInvocation;
            tipInvocationElement?.Toggle((GetDesktopWindow()));
            Marshal.ReleaseComObject(uiHostNoLaunch);

        }
    }

    [ComImport, Guid("4ce576fa-83dc-4F88-951c-9d0782b4e376")]
    class UIHostNoLaunch
    {
    }

    [ComImport, Guid("37c994e7-432b-4834-a2f7-dce1f13b834b")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface ITipInvocation
    {
        void Toggle(IntPtr hwnd);
    }

    [DllImport("user32.dll", SetLastError = false)]
    static extern IntPtr GetDesktopWindow();
}
