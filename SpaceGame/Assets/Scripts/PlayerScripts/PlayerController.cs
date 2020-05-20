using System;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public class PlayerController : MonoBehaviour
{
    [Header(" --- Setup --- ")] 
    [Tooltip("Hide or Show the gizmos")] 
    [SerializeField] private bool m_showGizmos = false;
    [Header(" --- Movement --- ")]

    [FormerlySerializedAs("Drag")]
    [Tooltip("Modify the Drag (aka how much the Spaceship slows down"),Range(0,1)]
    [SerializeField] private float m_drag = 0.95F;
    
    [FormerlySerializedAs("Acceleration")]
    [Tooltip("Modify the Acceleration of the Spaceship, 0.5F is default")]
    [SerializeField] private float m_acceleration = 0.5F;
    
    [FormerlySerializedAs("RotationSpeed")]
    [Tooltip("Modify the Rotation Speed of the Spacecraft")]
    [SerializeField] private float m_rotationSpeed = 2.0F;

    [Tooltip("By how much the spaceship banks")]
    [SerializeField] private float m_bankingFactor = 0.01F;
    [SerializeField] private Transform m_spaceShipModel;
    private Quaternion m_spaceShipModelInitialRotation = Quaternion.identity;
    
    private Vector3 m_speed;
    public const float MIN_EPSILON = 0.0001f;
    //gets called on state enter to reset player 
    public void ResetController()
    {
        transform.position = new Vector3(0, 0, transform.position.z);
        m_speed = Vector3.zero;
    }
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
            m_speed += dir * (m_acceleration * Time.deltaTime);

            //WOOP WOOP, BankAngle PullUP!
            float bankAngle = Mathf.Acos(Vector3.Dot(m_speed.normalized, dir.normalized));
            
            //add sign to bank-angle
            if (Vector3.Dot(Vector3.forward,Vector3.Cross(m_speed, dir))< 0)
            {
                bankAngle = -bankAngle;
            }
            
            //lazy initialize rotation
            if (m_spaceShipModelInitialRotation == Quaternion.identity)
            {
                m_spaceShipModelInitialRotation = m_spaceShipModel.localRotation;
            }

            //set bank
            var bank = Quaternion.AngleAxis(bankAngle * m_bankingFactor, Vector3.forward);
            m_spaceShipModel.localRotation =
                m_spaceShipModelInitialRotation * (bank.IsNaN() ? Quaternion.identity : bank);
                                      
            
            
            //assemble the quaternion for the new rotation
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle,Vector3.forward);

            //rotate towards the look-direction
            transform.rotation = Quaternion.Slerp(transform.rotation,rotation,Time.deltaTime * m_rotationSpeed);
        }
        
        //check if the speed is above our epsilon value and apply drag if it is
        if (Math.Abs(m_speed.y) >= MIN_EPSILON || Math.Abs(m_speed.x) >= MIN_EPSILON)
        {
            m_speed *= m_drag;
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
    public Vector3 Collide(float factor = 0.95f)
    {
        var transfer = m_speed * factor;
        m_speed *= 1 - factor;
        return transfer;
    }

    public Vector3 GetVelocity() => m_speed;
}
