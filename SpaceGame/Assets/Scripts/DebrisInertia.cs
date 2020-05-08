using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisInertia : MonoBehaviour
{
    private Quaternion m_inertia;
    void Start()
    {
        
        //create a random impulse
        m_inertia = Quaternion.AngleAxis(
            UnityEngine.Random.Range(-10, 10), 
            new Vector3(
                UnityEngine.Random.Range(-90, 90),
                UnityEngine.Random.Range(-90, 90),
                UnityEngine.Random.Range(-90, 90)
            ).normalized
        );
    }
    void Update()
    {
        //rotate using the random impulse
        var rotation = transform.rotation;
        rotation = Quaternion.Slerp(rotation,rotation*m_inertia,Time.deltaTime * 10) ;
        transform.rotation = rotation;
    }
}
