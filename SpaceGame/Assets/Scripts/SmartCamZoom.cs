using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tools;
using UnityEngine;

[Serializable]
public class CameraSetting
{
    [SerializeField] public int BarrelThreshold;
    [SerializeField] public float ZoomLevel;
}

public class SmartCamZoom : AUnityObserver
{
    [Header(" --- Setup ---")]
    
    [Tooltip("Hide or Show the gizmos")] 
    [SerializeField] private bool m_showGizmos = false;
    
    [Tooltip(
        "Object that create Playgrounds for your Debris, Obstacles etc...  they need to inherit from NotifyAddChildren")]
    [SerializeField]
    private List<NotifyAddChildren> m_collectionCreators = new List<NotifyAddChildren>();

    [Tooltip("The base zoom - level aka from 0 to N where N is the lowest threshold")] [SerializeField]
    private float m_baseZoom = 0;

    [Tooltip("The individual camera settings, if the number of barrels is above the threshold the settings is used")]
    [SerializeField]
    private List<CameraSetting> m_cameraSettings = new List<CameraSetting>();



    
    [Tooltip("the scan radius to check for barrels")] [SerializeField]
    private float m_radius = 10;

    private float m_currentSetting = 0;
    private float m_selectedSetting = 0;

    [Tooltip("The targeted camera")] [SerializeField]
    private Camera m_camera;

    private List<Transform> m_watchTransforms = new List<Transform>();

    private int m_currentlyCounted;

    private void Awake()
    {
        foreach (var nac in m_collectionCreators)
        {
            nac.Attach(this);
        }
    }

    void Start()
    {
        if (!m_camera)
            m_camera = Camera.main;
        
        m_cameraSettings = m_cameraSettings.OrderBy(setting => setting.BarrelThreshold).ToList();
        m_currentSetting = m_baseZoom;
    }

    private float m_begin;

    void Update()
    {
        ClearDeadTransforms();
        CountNearbyObjects();
        FindSetting();
        PositionCamera();
    }

    protected override void AGetUpdate(ISubject subject)
    {
        if (subject is NotifyAddChildren nac)
        {
            m_watchTransforms.Add(nac.LastAdded.transform);
        }
    }

    private void CountNearbyObjects()
    {
        m_currentlyCounted = 0;
        foreach (var tf in m_watchTransforms)
        {
            if(tf == null) continue;
            if (Vector3.Distance(tf.position, this.transform.position) < m_radius)
            {
                m_currentlyCounted++;
            }
        }
    }
    
    
    private void FindSetting()
    {

        float setting = m_baseZoom;
        
        foreach (var csetting in m_cameraSettings)
        {
            if (m_currentlyCounted >= csetting.BarrelThreshold)
            {
                setting = csetting.ZoomLevel;
            }
        }
        
        if (Math.Abs(setting - m_selectedSetting) > 0.01f)
        {
            m_selectedSetting = setting;
            m_begin = Time.time;
        }

        if (Time.time - m_begin > 1f)
        {
            m_currentSetting = m_selectedSetting;
        }
        
    }

    private void ClearDeadTransforms()
    {
        for (int i = 0; i < m_watchTransforms.Count; ++i)
        {
            if (m_watchTransforms[i] == null)
            {
                m_watchTransforms.RemoveAt(i);
            }
        }
    }


    private void PositionCamera()
    {
        var cpos = m_camera.transform.position;
        cpos.z = Mathf.Lerp(cpos.z, -m_currentSetting, Time.deltaTime);
        Debug.Log(cpos.z);
        m_camera.transform.position = cpos;
    }
#if (UNITY_EDITOR)
    void OnDrawGizmosSelected()
    {
        if (!m_showGizmos) return;
    
        //draw the radius where the device is put
        UnityEditor.Handles.DrawWireDisc(transform.position,Vector3.forward,m_radius);
        UnityEditor.Handles.Label(transform.position+Vector3.up*m_radius,"Camera Zoom Test Area");
        

        //draw the target position
        foreach (Transform tf in m_watchTransforms)
        {
            if(tf == null) continue;
            UnityEditor.Handles.color = Color.green;
            if (Vector3.Distance(tf.position, this.transform.position) < m_radius)
            {
                UnityEditor.Handles.color = Color.red;
            }
            
            UnityEditor.Handles.DrawWireDisc(tf.position,Vector3.forward,0.1f);
        }
      
    }
#endif
    
}