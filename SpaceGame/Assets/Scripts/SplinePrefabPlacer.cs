﻿using SplineMesh;
using UnityEditor;
using UnityEngine;


[RequireComponent(typeof(Spline))]
[ExecuteInEditMode]
public class SplinePrefabPlacer : MonoBehaviour
{
    private GameObject m_generated;
    private Spline m_spline;

    [SerializeField] 
    private GameObject m_prefab;

    [SerializeField] [Range(0.01F,1)]
    private float m_sampleSize;

    [SerializeField] private bool m_yeet;

    //where to generate all this nonesense
    //make sure that gameobject absolutely has to exist
    public GameObject Generated
    {
        get
        {
            if (m_generated == null)
            {
                string generatedName = "generated by " + GetType().Name;
                var generatedTranform = transform.Find(generatedName);
                m_generated = generatedTranform != null ? generatedTranform.gameObject : Instantiate(new GameObject(generatedName),gameObject.transform);
                m_generated.name = generatedName;
            }

            return m_generated;
        }
        set => m_generated = value;
    }

    //make sure our update loop is respected
    private void OnEnable()
    {
        m_spline = GetComponent<Spline>();
#if UNITY_EDITOR
        EditorApplication.update += EditorUpdate;
#endif
    }

    //and discarded accordingly
    void OnDisable() {
#if UNITY_EDITOR
        EditorApplication.update -= EditorUpdate;
#endif
    }

    private bool generated = false;
    
    //check if we should regenerate by
    //asking the user if he yote his stuff
    public void EditorUpdate()
    {
        if (!m_yeet && !generated)
        {
            Generate();
            generated = true;
        }
        else if (m_yeet)
        {
            generated = false;
        }
    }

    
    //generate instances
    public void Generate()
    {
        for (float pos = 0; pos < m_spline.nodes.Count - 1; pos += m_sampleSize)
        {
            CurveSample sample = m_spline.GetSample(pos);
            Instantiate(m_prefab, sample.location, sample.Rotation, Generated.transform);
        }
    }
    
    #if UNITY_EDITOR
    //give a visual indication of where the stuff is roughly going to be
    private void OnDrawGizmos()
    {
        if(m_spline)
            for (float pos = 0; pos < m_spline.nodes.Count - 1; pos += m_sampleSize)
            {
                CurveSample sample = m_spline.GetSample(pos);
                Handles.DrawWireDisc(sample.location, Vector3.back, 1F);
            }
    }
    #endif
}
