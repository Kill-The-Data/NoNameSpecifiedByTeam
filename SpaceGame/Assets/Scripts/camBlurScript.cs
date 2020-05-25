using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security.AccessControl;
using UnityEngine;

public class camBlurScript : MonoBehaviour
{

    [SerializeField] private Camera m_renderTexCam = null;
    [SerializeField] private GameObject m_BlurrObject = null;

    private Camera m_MainCam = null;
    // private LayerMask m_blurMask = 

    private bool m_isActive = false;

    private LayerMask m_OriginalLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        m_MainCam = Camera.main;
        m_OriginalLayerMask = m_MainCam.cullingMask;
        m_BlurrObject?.SetActive(false);
        m_renderTexCam?.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("b"))
            ChangeBlur();

    }

    private void ChangeBlur()
    {
        Debug.Log("Changing blur");
        if (m_isActive)
            DeactivateBlur();
        else
            ActivateBlur();
    }
    private void ActivateBlur()
    {
        m_BlurrObject?.SetActive(true);
        m_renderTexCam?.gameObject.SetActive(true);

        m_MainCam.cullingMask = LayerMask.GetMask("Ignore Raycast");
      m_isActive = true;

}

private void DeactivateBlur()
    {
        Debug.Log("deact blur");

        m_BlurrObject?.SetActive(false);
        m_renderTexCam?.gameObject.SetActive(false);
        m_MainCam.cullingMask = m_OriginalLayerMask;
        m_isActive = false;

    }
}
