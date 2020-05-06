using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CamScroll : MonoBehaviour
{

    [Header(" --- Setup --- ")] 
    
    [Tooltip("The camera that should be affected by the Scrolling")]
    [SerializeField] private Camera m_camera;

    [Tooltip(
        "This is the Player Driver from which the speed will be matched, if it is null\nI will try to find it in the current GO")]
    [SerializeField] private PlayerController m_playerController = null;

    [Header(" --- Gameplay ---")]


    [Tooltip("The offset to the sides of the view-frustum from which to start scrolling"),Range(100,400)]
    [SerializeField] private float m_border = 200F;


    [Tooltip("how fast the camera moves to the position it should be at"), Range(0,2)]
    [SerializeField] private float m_camSpeed = 1F;

    // Start is called before the first frame update
    void Start()
    {
        if(m_camera == null) m_camera = Camera.main;
        if(m_camera == null) Debug.LogError("Scrolling camera does not have associated Camera set");
        if(m_playerController == null) m_playerController = GetComponent<PlayerController>();
        if(m_playerController == null) Debug.LogError("Player Controller was null & not part of the current GO's setup");
    }

    // Update is called once per frame
    void Update()
    {

        var dir = m_camera.transform.position - transform.position;
        dir.z = 0;

        if (Mathf.Abs(dir.magnitude) > m_border)
        {
            m_camera.transform.position -= dir * m_camSpeed * Time.deltaTime;
        }
    }
}
