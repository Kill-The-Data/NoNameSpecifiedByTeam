using UnityEngine;

[RequireComponent(typeof(NPCController))]
public class WaypointNavigator : MonoBehaviour
{

    [SerializeField] private Waypoint m_start;

    private Waypoint m_current;

    private NPCController m_controller;

    void Start()
    {
        m_controller = GetComponent<NPCController>();
        m_controller.Destination = m_start.GetPosition();
        m_current = m_start;
    }
    protected virtual void ReachedEnd()
    {
        m_current = m_start;
    }
    protected void Move()
    {
        if (m_controller.HasReachedDestination)
        {
            if (m_current.NextWaypoint)
            {
                m_current = m_current.NextWaypoint;
            }
            else
            {
                ReachedEnd();
            }
            m_controller.Destination = m_current.GetPosition();

        }
    }
    protected virtual void Update()
    {
        Move();
    }
}
