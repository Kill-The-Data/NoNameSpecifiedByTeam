using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterRotation : MonoBehaviour
{
    private Vector3 m_randomRot;
    [SerializeField] private float m_speed = 100.0f;
    void Start()
    {
        Vector2 vec2 = Random.insideUnitCircle;
        m_randomRot = new Vector3(vec2.x, 1,vec2.y);
        Debug.Log(m_randomRot);
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, m_randomRot, Time.deltaTime * m_speed);
    }
}
