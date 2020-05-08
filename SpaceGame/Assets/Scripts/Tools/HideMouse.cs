using UnityEngine;

public class HideMouse : MonoBehaviour
{
    void Start()
    {
        //this is set via the Editor Script
        if(AutoHide)
            Cursor.visible = false;
    }

    public bool AutoHide;
}
