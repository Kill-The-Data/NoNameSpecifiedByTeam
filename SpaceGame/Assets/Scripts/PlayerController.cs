using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public class PlayerController : MonoBehaviour
{
    private Vector3 m_speed;
    public const float MIN_EPSILON = 0.0001f;
    
    
    public float drag = 0.95F;
    public float acceleration = 0.5F;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.W))
        {
            m_speed += Vector3.up * acceleration * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            m_speed += Vector3.down * acceleration * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            m_speed += Vector3.left * acceleration * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            m_speed += Vector3.right * acceleration * Time.deltaTime;
        }

       


        if (Input.GetMouseButton(0))
        {


            Vector2 mPos = Input.mousePosition;
            Vector3 wPos = Camera.main.ScreenToWorldPoint(new Vector3(mPos.x,mPos.y,transform.position.z));

            wPos.z = transform.position.z;


            var dir = wPos - transform.position;
            dir.Normalize();



            m_speed += dir * acceleration * Time.deltaTime;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;


            Quaternion rotation = Quaternion.AngleAxis(angle,Vector3.forward);

            transform.rotation = Quaternion.Slerp(transform.rotation,rotation,Time.deltaTime);



            // transform.position = wPos;
        }
        
        if (Math.Abs(m_speed.y) >= MIN_EPSILON || Math.Abs(m_speed.x) >= MIN_EPSILON)
        {
            m_speed *= drag;
        } else {
            m_speed = Vector3.zero;
        }

        transform.position += m_speed * Time.deltaTime;
    }
}
