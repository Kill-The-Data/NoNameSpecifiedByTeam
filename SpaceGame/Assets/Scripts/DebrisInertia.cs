using UnityEngine;

public class DebrisInertia : MonoBehaviour
{
    [SerializeField] private Vector3 m_rotationAxis = Vector3.zero;
    [SerializeField] private float m_angle = 0;
    
    private Quaternion m_inertia;
    void Start()
    {
        if (m_rotationAxis == Vector3.zero && m_angle == 0)
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
        else m_inertia = Quaternion.AngleAxis(m_angle, m_rotationAxis);
    }
    void Update()
    {
        //rotate using the random impulse
        var rotation = transform.rotation;
        rotation = Quaternion.Slerp(rotation,rotation*m_inertia,Time.deltaTime * 10) ;
        transform.rotation = rotation;
    }
}
