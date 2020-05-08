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
        
        //for tab-tip a few extra things need to happen
        //tabtip.exe is actually only the com-server
        if (method == Method.TabTip)
        {
            //create the com-object
            var uiHostNoLaunch = new UIHostNoLaunch();
            
            //we assume that the com-object we just created implements ITipInvocation
            var tipInvocationElement = uiHostNoLaunch as ITipInvocation;
            
            //invoke TabTip, the handler we specify is the Desktop Window
            tipInvocationElement?.Toggle((GetDesktopWindow()));
            
            //since the com-object was created manually we also need to destroy it manually & explicitly
            Marshal.ReleaseComObject(uiHostNoLaunch);

        }
    }

    //some smelly stuff 
    //the tab-tip com interface is not documented so
    //thanks to the power of the internet we found 
    //it anyways and are now targeting it via guids
    
    //com object
    [ComImport, Guid("4ce576fa-83dc-4F88-951c-9d0782b4e376")]
    class UIHostNoLaunch
    {
    }

    //tab-tip invocation-element
    [ComImport, Guid("37c994e7-432b-4834-a2f7-dce1f13b834b")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface ITipInvocation
    {
        void Toggle(IntPtr hwnd);
    }
    
    //we also need the desktop window (luckily most users have one) 
    //therefore we can assume to find one associated
    //with user32.dll
    [DllImport("user32.dll", SetLastError = false)]
    static extern IntPtr GetDesktopWindow();
}
