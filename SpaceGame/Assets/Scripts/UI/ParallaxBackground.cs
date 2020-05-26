using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Header(" --- Setup ---")]

    [Tooltip("This is the GameObject from which the parallax is driven, the more it goes to the right the more the parallax goes to the left etc...")]
    [SerializeField] private Transform m_parallaxDriver = null;

    [Header(" --- Parallax ---")]

    [Tooltip("This is by how much the parallax moves in perspective to the Driver"),Range(0,1)]
    [SerializeField] private float m_displacementFactor = 0.1F;

    private Vector3 m_initialOffset;
    private Vector3 m_selfOrigPosition;

    
    //On setup assign the self orig position and the orig position of the driver
    void Start()
    {
        m_selfOrigPosition = transform.position;

        if (m_parallaxDriver)
        {
            m_initialOffset = m_parallaxDriver.position;
        }
    }

    //on update set the position of the target to (driver-delta * displacement) + orig-position
    void Update()
    {
        if (m_parallaxDriver)
        {
            var diff = m_parallaxDriver.position - m_initialOffset;
            diff *= -m_displacementFactor;
            transform.position = m_selfOrigPosition + diff;

        }
    }
}
