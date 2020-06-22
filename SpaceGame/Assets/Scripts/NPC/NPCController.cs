using System;
using UnityEngine;

public class NPCController : MonoBehaviour
{

    [Range(0.1f,30)]
    [SerializeField] private float m_acceleration = 1f;
    
    [Range(1,30)]
    [SerializeField] private float m_maxSpeed = 5f;
    
    [Range(0.1f,1f)]
    [SerializeField] private float m_minDistanceToDestinations = 0.1f;

    private Vector3 m_speed;
    [SerializeField] private bool m_Rotate=true;

    [SerializeField] private Transform m_follower;
    
    public bool HasReachedDestination { get; private set; }
    private Vector3 DestinationImpl = Vector3.zero;
    public Vector3 Destination
    {
        get => DestinationImpl;
        set
        {
            HasReachedDestination = false;
            DestinationImpl = value;
        }
    }
    public bool IsMoving = true;

    private void Awake()
    {
        if (!m_follower) m_follower = transform;
    }

    public void Update()
    {
        if (!IsMoving) return;
        if(Destination != Vector3.zero && !HasReachedDestination)
        {
            var currentPosition = transform.position;
            var direction = (Destination - currentPosition).normalized;

            if (m_Rotate)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                //rotate towards the look-direction
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);
            }
                      
            if (Vector3.Distance(currentPosition, Destination) > m_minDistanceToDestinations)
            {
                float maxSpeed = m_maxSpeed / Mathf.Max(1,Vector3.Distance(m_follower.position, transform.position) / 10f);
                m_speed += direction * m_acceleration;
                if (m_speed.magnitude > maxSpeed) m_speed = m_speed.normalized * maxSpeed;
            }
            else
            {
                HasReachedDestination = true;
            }
            
        }

        transform.position += m_speed * Time.deltaTime;

    }
}
