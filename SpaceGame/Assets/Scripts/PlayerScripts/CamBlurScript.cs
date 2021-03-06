using UnityEngine;

public class CamBlurScript : MonoBehaviour
{

    [SerializeField] private Camera m_renderTexCam = null;
    [SerializeField] private GameObject m_BlurrObject = null;

    private Camera m_MainCam = null;

    private bool m_isActive = false;

    private LayerMask m_OriginalLayerMask;
    void Awake()
    {
        //get main cam
        m_MainCam = Camera.main;
        //layer to go back to after blur is disabled
        m_OriginalLayerMask = m_MainCam.cullingMask;
        m_BlurrObject?.SetActive(false);
        m_renderTexCam?.gameObject.SetActive(false);
    }

    void Update()
    {
        //test input
        if (Input.GetKeyDown("b"))
            ChangeBlur();
    }

    //changes blur state
    public void ChangeBlur()
    {
        if (m_isActive)
            DeactivateBlur();
        else
            ActivateBlur();
    }
    public void ActivateBlur()
    {
        m_BlurrObject?.SetActive(true);
        m_renderTexCam?.gameObject.SetActive(true);
        if (m_MainCam)
            m_MainCam.cullingMask = LayerMask.GetMask("Ignore Raycast");
        m_isActive = true;

    }

    public void DeactivateBlur()
    {
        m_BlurrObject?.SetActive(false);
        m_renderTexCam?.gameObject.SetActive(false);
        if (m_MainCam)
            m_MainCam.cullingMask = m_OriginalLayerMask;
        m_isActive = false;
    }
}
