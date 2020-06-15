using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Handles reseting ship on enable =>gets called on tutorial start
/// activates movement & visability once we get the first input
/// </summary>
[RequireComponent(typeof(NPCController))]
[RequireComponent(typeof(WaypointNavigator))]
public class GhostShipController : MonoBehaviour
{
    private WaypointNavigator m_waypointNavigator = null;
    private MeshRenderer m_MR = null;
    private bool m_moving = false;

    private void Awake()
    {
        m_waypointNavigator = GetComponent<WaypointNavigator>();
        m_MR = GetComponent<MeshRenderer>();
    }
    //reset on enable
    private void OnEnable()
    {
        m_moving = false;
        m_MR.enabled = false;
        m_waypointNavigator?.Reset();
    }
    //check for input if we have not gotten any yet
    public void Update()
    {
        if (!m_moving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_MR.enabled=true;
                m_waypointNavigator?.StartMove();
                m_moving = true;
            }
        }
    }
}
