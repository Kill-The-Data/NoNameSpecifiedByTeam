using System;
using UnityEngine;

[RequireComponent(typeof(NPCController))]
public class WaypointNavigator : MonoBehaviour
{

    [SerializeField] protected Waypoint m_start;

    [SerializeField] private bool m_ResetStartToStartNode = false;
    protected Waypoint m_current;

    private NPCController m_controller = null;
    public bool IsMoving = false;


    private void OnEnable()
    {
        if (!m_controller)
            m_controller = GetComponent<NPCController>();
        Reset();
    }
    
    private void Awake()
    {
        m_controller = GetComponent<NPCController>();
    }

    public void Reset()
    {
        m_controller.Destination = m_start.GetPosition();
        m_current = m_start;
        if (m_ResetStartToStartNode) transform.position = m_start.GetPosition();
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
        if (IsMoving)
            Move();
        else
            m_controller.IsMoving = false;
    }
    public void StartMove()
    {
        IsMoving = true;
        m_controller.IsMoving = true;

    }
}
