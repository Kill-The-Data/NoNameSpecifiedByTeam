using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShipHomingDeviceUI : MonoBehaviour
{
    [Header(" --- Setup ---")]
    
    [Tooltip("Hide or Show the gizmos")] 
    [SerializeField] private bool m_showGizmos = false;

    [Tooltip("The Mothership to home")]
    [SerializeField] private Transform m_target = null;  
    

    [Tooltip("The child that should be the homing device")]
    [SerializeField]  Transform m_homingDeviceChild = null;

    [Header(" --- UI Settings ---")] 
    
    [Tooltip("The radius of the arrow to the ship")]
    [SerializeField] private float m_radius = 2;

    [Tooltip("The radius from where on the homing device will be hidden")] 
    [SerializeField] private float m_hideRadius = 1;


    private Vector3 m_direction;

    void Start()
    {
        if(!m_target) Debug.LogError("Homing needs something to home");

    }

    void Update()
    {
        m_direction = (transform.position - m_target.position);

        var position = m_direction.normalized * m_radius;
        m_homingDeviceChild.position = transform.position - position;

        //assemble the quaternion for the new rotation
        float angle = Mathf.Atan2(m_direction.normalized.y, m_direction.normalized.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle,Vector3.forward);

        m_homingDeviceChild.rotation = rotation;

        m_homingDeviceChild.gameObject.SetActive(!(m_direction.magnitude < m_hideRadius));
    }

    void OnDrawGizmosSelected()
    {
        if (!m_showGizmos) return;
        UnityEditor.Handles.DrawWireDisc(transform.position,Vector3.forward,m_radius);
        UnityEditor.Handles.Label(transform.position+m_direction.normalized*m_radius,"Arrow Radius");

        UnityEditor.Handles.color = Color.yellow;

        UnityEditor.Handles.DrawWireDisc(transform.position,Vector3.forward,m_hideRadius);
        UnityEditor.Handles.Label(transform.position+m_direction.normalized*m_hideRadius,"Unaffected Area");

        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(m_target.position,Vector3.forward,0.1f);
    }
}
