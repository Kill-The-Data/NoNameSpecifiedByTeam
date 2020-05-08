using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CamScroll : MonoBehaviour
{
    //----------- Setup Variables 
    [Header(" --- Setup --- ")] 
    
    [Tooltip("Hide or Show the gizmos")] 
    [SerializeField] private bool m_showGizmos = false;

    [Tooltip("The camera that should be affected by the Scrolling")]
    [SerializeField] private Camera m_camera;


  
    
    //----------- Gameplay Variables
    [Header(" --- Gameplay ---")]
    
    [Tooltip("The offset to the sides of the view-frustum from which to start scrolling"),Range(0,2)]
    [SerializeField] private float m_border = 0.5F;


    [Tooltip("how fast the camera moves to the position it should be at min"), Range(0, 6)] 
    [SerializeField] private float m_camSpeedMin = 0F;
    
    [Tooltip("how fast the camera moves to the position it should be at max"), Range(0,6)]
    [SerializeField] private float m_camSpeedMax = 1F;

    [Tooltip("The Dividend for the Lerp Intervall (aka how fast to go from min to max speed)")]
    [SerializeField] private float m_criticalSection = 10F;
    
    //----------- Unexposed Variables
    private Vector3 m_direction;

    void Start()
    {
        //check if the camera was reassigned otherwise set it to the main camera
        if(m_camera == null) m_camera = Camera.main;
        if(m_camera == null) Debug.LogError("Scrolling camera does not have associated Camera set");
        
    }

    void Update()
    {
        //get the direction to the camera
        m_direction = m_camera.transform.position - transform.position;
        m_direction.z = 0;

        //check if the camera center is further than the preset-distance and scroll along with the target
        if (Mathf.Abs(m_direction.magnitude) > m_border)
        {
            //check where on the curve we roughly are
            var lerpFactor = Mathf.Min(Mathf.Abs(m_direction.magnitude / m_criticalSection), 1);
            //get the value for the cameraSpeed
            var camSpeed = Mathf.Lerp(m_camSpeedMin, m_camSpeedMax,lerpFactor);
            //apply direction vector * speed
            m_camera.transform.position -= m_direction * (camSpeed * Time.deltaTime);
        }
    }
    #if (UNITY_EDITOR)

    void OnDrawGizmosSelected()
    {
        //check if we should display the gizmos
        if (!m_showGizmos) return;
        var position = transform.position;
        
        //show the radius of the no move zone
        UnityEditor.Handles.DrawWireDisc(position,Vector3.forward,m_border);
        UnityEditor.Handles.Label(position+m_direction.normalized*m_border,"Cam Radius");
    }
    #endif
}
