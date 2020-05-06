using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShipHomingDeviceUI : MonoBehaviour
{
    [Header(" --- Setup ---")]
    
    [Tooltip("The Mothership to home")]
    [SerializeField] private Transform m_target = null;

    [Tooltip("The child that should be the homing device")]
    [SerializeField]  Transform m_homingDeviceChild = null;

    [Header(" --- UI Settings ---")] 
    
    [Tooltip("The radius of the arrow to the ship")]
    [SerializeField] private float m_radius = 2;

    [Tooltip("The radius from where on the homing device will be hidden")] 
    [SerializeField] private float m_hideRadius = 1;
   
    void Start()
    {
        if(!m_target) Debug.LogError("Homing needs something to home");

    }

    void Update()
    {
        var dir = (transform.position - m_target.position);

        var position = dir.normalized * m_radius;
        m_homingDeviceChild.position = transform.position - position;

        //assemble the quaternion for the new rotation
        float angle = Mathf.Atan2(dir.normalized.y, dir.normalized.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle,Vector3.forward);

        m_homingDeviceChild.rotation = rotation;

        if (dir.magnitude < m_hideRadius)
        {
            m_homingDeviceChild.gameObject.SetActive(false);
        }
        else
        {
            m_homingDeviceChild.gameObject.SetActive(true);
        }

    }
}
