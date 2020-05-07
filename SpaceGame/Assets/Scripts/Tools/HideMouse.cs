using UnityEngine;

public class HideMouse : MonoBehaviour
{
    void Start()
    {
        if(AutoHide)
            Cursor.visible = false;
    }

    public bool AutoHide;
}
