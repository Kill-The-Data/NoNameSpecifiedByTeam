using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DisableAnimation : MonoBehaviour
{

    private Animator m_Animator = null;
    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        m_Animator?.ResetTrigger("TriggerAnimation");
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }
    
}
