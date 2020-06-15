using UnityEngine;

public class BuoySway : MonoBehaviour 
{
    [SerializeField] private Vector2 m_direction;
    [SerializeField] private float m_impulse;
    [SerializeField] private MotherShipCollisionHandler m_collisionHandler;
    [SerializeField] private PlayerController m_playerController;
    [SerializeField] private float m_heft;
    
    
    private Vector2 collisionImpulse;

    private float m_time;

    private Quaternion m_initialRotation;
    
    private void Awake()
    {

        m_collisionHandler.collision += Collide;
        m_initialRotation = transform.localRotation;
        m_direction *= m_heft;
    }

    void Update()
    {
        m_time += Time.deltaTime;
        
        //get angle
        var Phi = Mathf.Sin(m_time) * m_impulse;
        
        //get rotation axis
        var Axis = Vector2.Perpendicular(m_direction.normalized);
        
        //rotate around axis
        transform.localRotation = m_initialRotation * Quaternion.AngleAxis(Phi,new Vector3(Axis.x,0,Axis.y));

    }

    private void Collide(ISubject subject)
    {
        var v3 = m_playerController.GetVelocity();
        
        m_direction.x += v3.x;
        m_direction.y += v3.y;
    }
}
