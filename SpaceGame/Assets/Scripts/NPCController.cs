using UnityEngine;

public class NPCController : MonoBehaviour
{

    [Range(0.1f,20)]
    [SerializeField] private float m_acceleration = 1f;
    
    [Range(1,20)]
    [SerializeField] private float m_maxSpeed = 5f;
    
    [Range(0.1f,1f)]
    [SerializeField] private float m_minDistanceToDestinations = 0.1f;

    private Vector3 m_speed;
    
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


    public void Update()
    {
        if(Destination != Vector3.zero && !HasReachedDestination)
        {
            var currentPosition = transform.position;
            var direction = (Destination - currentPosition).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle,Vector3.forward);
            //rotate towards the look-direction
            transform.rotation = Quaternion.Slerp(transform.rotation,rotation,Time.deltaTime * 2);
            
            if (Vector3.Distance(currentPosition, Destination) > m_minDistanceToDestinations)
            {
                m_speed += direction * m_acceleration;

                if (m_speed.magnitude > m_maxSpeed) m_speed = m_speed.normalized * m_maxSpeed;
            }
            else
            {
                HasReachedDestination = true;
            }
            
        }

        transform.position += m_speed * Time.deltaTime;

    }
}
