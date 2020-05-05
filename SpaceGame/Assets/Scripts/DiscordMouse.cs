using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscordMouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        
        Vector2 mPos = Input.mousePosition;
        Vector3 wPos = Camera.main.ScreenToWorldPoint(new Vector3(mPos.x,mPos.y,transform.position.z));

        wPos.z = transform.position.z;

        transform.position = wPos;
    }
}
