﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldState : MonoBehaviour
{
    [SerializeField] private GameObject ForceShield;
    [SerializeField] private float fadeSpeed = 0.5f;
    
    public bool IsActive
    {
        get;
        private set;
    } = false;

    public void Init()
    {
        FadeOut();
    }
    void Update()
    {
        #if UNITY_EDITOR
            if (Input.GetKey("space")) FadeOut();
        #endif
    }
    public void ActivateShield()
    {
        IsActive = true;
        ForceShield.SetActive(true);
        ForceShield.GetComponent<Dissolve>()?.StartReverseDissolve();
    }
    public  void DeactivateShield() { FadeOut();}
    private void FadeOut()
    {
        ForceShield.GetComponent<Dissolve>()?.StartDissolve();
        IsActive = false;
    }

}
