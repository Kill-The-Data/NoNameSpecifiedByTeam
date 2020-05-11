using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashMovementController : MonoBehaviour
{

    public float Drag = 0.995f;
    public Vector3 Speed = Vector3.zero;
    
    // Update is called once per frame
    void Update()
    {
        transform.position += Speed * Time.deltaTime;
        Speed *= Drag;
    }
}
