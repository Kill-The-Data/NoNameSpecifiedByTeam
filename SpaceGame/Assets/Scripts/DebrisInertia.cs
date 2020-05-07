using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisInertia : MonoBehaviour
{
    private Quaternion m_inertia;
    // Start is called before the first frame update
    void Start()
    {
        m_inertia = Quaternion.AngleAxis(
            UnityEngine.Random.Range(-10, 10), 
            new Vector3(
                UnityEngine.Random.Range(-90, 90),
                UnityEngine.Random.Range(-90, 90),
                UnityEngine.Random.Range(-90, 90)
            ).normalized
        );
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,transform.rotation*m_inertia,Time.deltaTime * 10) ;
    }
}
