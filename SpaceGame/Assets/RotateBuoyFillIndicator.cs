using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class RotateBuoyFillIndicator : MonoBehaviour
{
    [SerializeField]private  UpdateBuoyText m_buoyText = null;
    public void UpdateRotation(float fillAmount)
    {
        m_buoyText?.UpdateText(fillAmount);

        var rotation = fillAmount * -360;
        transform.localRotation = Quaternion.Euler(0, Mathf.Clamp(rotation, -360, 0), 0);
    }
    public void OnEnable()
    {
        Reset();
    }
    private void Reset()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 0);
     
    }
}
