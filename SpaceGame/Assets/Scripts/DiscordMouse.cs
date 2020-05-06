using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscordMouse : MonoBehaviour
{
    //Follow the mouse @see PlayerController.Update
    void Update()
    { 
        
        Vector2 mPos = Input.mousePosition;
        Vector3 wPos = Camera.main.ScreenToWorldPoint(new Vector3(mPos.x,mPos.y,transform.position.z));

        wPos.z = transform.position.z;

        transform.position = wPos;
    }
}
