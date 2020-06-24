﻿using UnityEngine;
public class RotateBuoyFillIndicator : MonoBehaviour
{
    [SerializeField]private  UpdateBuoyText m_buoyText = null;
    [SerializeField] private bool m_RotateComponent = false;
    public void UpdateRotation(float fillAmount)
    {
        m_buoyText?.UpdateText(fillAmount);
        if (!m_RotateComponent) return;
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
