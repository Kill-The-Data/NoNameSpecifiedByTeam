using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thrusterRotator : MonoBehaviour
{
    public Transform T = null;
    public Renderer R = null;
    // Update is called once per frame
    void Update()
    {
        if(R && T)
        {
            R.material.SetFloat("ROTATION", T.eulerAngles.y);
        }
    }
}
