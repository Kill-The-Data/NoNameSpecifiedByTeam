using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ReturnToBaseTrash : MonoBehaviour
{
    private Vector3 m_initialPosition;
    [SerializeField] private float radius = 20;

    private TrashMovementController fuck;
    
    void Start()
    {
        m_initialPosition = transform.position;
        fuck = GetComponent<TrashMovementController>();
    }

    private void Update()
    {
        if (Vector3.Distance(m_initialPosition, transform.position) > radius)
        {
            transform.position = m_initialPosition;
            fuck.ingStop();
        }
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(m_initialPosition,Vector3.back, radius);
    }


#endif
    
}
