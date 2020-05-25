using System.Collections.Generic;
using UnityEngine;

public class MotherShipHomingDeviceUI : MonoBehaviour
{
    //----------- Setup Variables
    [Header(" --- Setup ---")]
    
    [Tooltip("Hide or Show the gizmos")] 
    [SerializeField] private bool m_showGizmos = false;

    [Tooltip("The Motherships to home")]
    [SerializeField] private List<BuoyFillUp> m_targets = new List<BuoyFillUp>(1);  
    
    [Tooltip("The child that should be the homing device")]
    [SerializeField]  Transform m_homingDeviceChild = null;

    //----------- UI Settings Variables
    [Header(" --- UI Settings ---")] 
    
    [Tooltip("The radius of the arrow to the ship")]
    [SerializeField] private float m_radius = 2;

    [Tooltip("The radius from where on the homing device will be hidden")] 
    [SerializeField] private float m_hideRadius = 1;

    //----------- Unexposed Variables
    private Vector3 m_direction;

    void Start()
    {
        if(m_targets.Count == 0) Debug.LogError("Homing needs something to home");
    }

    void Update()
    {
        Transform target;
        Vector3 closest = Vector3.positiveInfinity;
        
        foreach (BuoyFillUp buoyFillUp in m_targets)
        {
            var dist = transform.position - buoyFillUp.transform.position;
            if(buoyFillUp.Full()) continue;
            
            if (dist.magnitude < closest.magnitude)
            {
                closest = dist;
            }
        }
        
        //get the direction to home
        m_direction = closest;
        
        //check if the closest spacestation is actually a station
        if(closest != Vector3.positiveInfinity)
        {
            //put the homing-device at the right position
            var position = m_direction.normalized * m_radius;
            m_homingDeviceChild.position = transform.position - position;
        }
        //assemble the quaternion for the new rotation
        float angle = Mathf.Atan2(m_direction.normalized.y, m_direction.normalized.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle,Vector3.forward);

        m_homingDeviceChild.rotation = rotation;
        
        //check if the target is within the radius where it should not be homed
        m_homingDeviceChild.gameObject.SetActive(!(m_direction.magnitude < m_hideRadius));
    }
    
    #if (UNITY_EDITOR)
    void OnDrawGizmosSelected()
    {
        if (!m_showGizmos) return;
    
        //draw the radius where the device is put
        UnityEditor.Handles.DrawWireDisc(transform.position,Vector3.forward,m_radius);
        UnityEditor.Handles.Label(transform.position+m_direction.normalized*m_radius,"Arrow Radius");

        //draw the radius at which the device will be hidden 
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position,Vector3.forward,m_hideRadius);
        UnityEditor.Handles.Label(transform.position+m_direction.normalized*m_hideRadius,"Unaffected Area");

        //draw the target position
        foreach (BuoyFillUp buoyFillUp in m_targets)
        {
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(buoyFillUp.transform.position,Vector3.forward,0.1f);
        }
      
    }
    #endif
}
