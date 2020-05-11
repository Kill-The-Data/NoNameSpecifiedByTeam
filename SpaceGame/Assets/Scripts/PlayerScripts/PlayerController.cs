using System;
using UnityEngine;

using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public class PlayerController : MonoBehaviour
{
    [Header(" --- Setup --- ")] 
    [Tooltip("Hide or Show the gizmos")] 
    [SerializeField] private bool m_showGizmos = false;
    
    [Header(" --- Movement --- ")]

    [Tooltip("Modify the Drag (aka how much the Spaceship slows down"),Range(0,1)]
    public float Drag = 0.95F;
    [Tooltip("Modify the Acceleration of the Spaceship, 0.5F is default")]
    public float Acceleration = 0.5F;
    [Tooltip("Modify the Rotation Speed of the Spacecraft")]
    public float RotationSpeed = 2.0F;
    
    private Vector3 m_speed;
    public const float MIN_EPSILON = 0.0001f;
    
    void Update()
    {
        
        //check if the mouse is held down
        if (Input.GetMouseButton(0))
        {
            var pPos = transform.position;
            
            //transform the mouse position to game coordinates
            Vector2 mPos = Input.mousePosition;
            Vector3 wPos = Camera.main.ScreenToWorldPoint(new Vector3(mPos.x,mPos.y,pPos.z));
            wPos.z = pPos.z;

            //get the relative direction
            var dir = wPos - pPos; 
            dir.Normalize();

            //accelerate towards the direction
            m_speed += dir * (Acceleration * Time.deltaTime);
            
            //assemble the quaternion for the new rotation
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle,Vector3.forward);

            //rotate towards the look-direction
            transform.rotation = Quaternion.Slerp(transform.rotation,rotation,Time.deltaTime * RotationSpeed);
        }
        
        //check if the speed is above our epsilon value and apply drag if it is
        if (Math.Abs(m_speed.y) >= MIN_EPSILON || Math.Abs(m_speed.x) >= MIN_EPSILON)
        {
            m_speed *= Drag;
        } else {
            //below a certain epsilon simply set the speed to 0
            m_speed = Vector3.zero;
        }

        //do euler step
        transform.position += m_speed * Time.deltaTime;
    }

    #if (UNITY_EDITOR)

    void OnDrawGizmosSelected()
    {
        if (!m_showGizmos) return;
        var position = transform.position;
        UnityEditor.Handles.DrawLine(position,position+m_speed);
        UnityEditor.Handles.DrawWireDisc(position+m_speed,Vector3.back, 0.1f);
    }

    #endif
}
