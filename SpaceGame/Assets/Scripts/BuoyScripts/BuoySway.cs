using UnityEngine;
[RequireComponent(typeof(MotherShipCollisionHandler))]
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
        m_collisionHandler = GetComponent<MotherShipCollisionHandler>();
        m_collisionHandler.collision += Collide;
        m_initialRotation = transform.localRotation;
        m_direction *= m_heft;
    }

    void Update()
    {
        m_time += Time.deltaTime;

        var Zeta = Mathf.Sin(m_time) * m_impulse;

        var Axis = Vector2.Perpendicular(m_direction.normalized);
        
        transform.localRotation = m_initialRotation * Quaternion.AngleAxis(Zeta,new Vector3(Axis.x,0,Axis.y));

    }

    private void Collide(ISubject subject)
    {
        var v3 = m_playerController.GetVelocity();
        
        m_direction.x += v3.x;
        m_direction.y += v3.y;
    }
}
