using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBuoyFillIndicator : MonoBehaviour
{

    public float angle = 0;
    public Transform center;
    public Vector3 rotationCenter = new Vector3(0, 0, 0);
    public void Update()
    {
        if (Input.GetKeyDown("space"))
        {

            angle = Mathf.Lerp(angle, angle + 10, 10);
        }
        //transform.RotateAround(Vector3.forward, angle);
        transform.localRotation = Quaternion.EulerAngles(0, angle, 0);
    }

    public void UpdateRotation()
    {

    }

}
