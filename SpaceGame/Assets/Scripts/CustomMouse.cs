using UnityEngine;
public class CustomMouse : MonoBehaviour
{
    public Texture2D CursorTexture;
    public CursorMode CursorMode = CursorMode.Auto;
    public Vector2 HotSpot = Vector2.zero;

    void Start()
    {
        //Set Cursor Texture
        Cursor.SetCursor(CursorTexture,HotSpot,CursorMode);
    }
}
