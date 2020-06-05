using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Update()
    {
        if (m_controller.HasReachedDestination)
        {
            m_current = m_current.NextWaypoint != null ? m_current.NextWaypoint : m_start;
            m_controller.Destination = m_current.GetPosition();
        }
    }
}
