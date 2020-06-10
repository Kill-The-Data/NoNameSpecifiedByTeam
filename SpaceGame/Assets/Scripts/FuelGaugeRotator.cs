using System;
using UnityEngine;
using UnityEngine.UI;

public class FuelGaugeRotator : MonoBehaviour
{
    //fill factor
    private float amount = 1;
    //radial slider image reference
    [SerializeField] private Image m_Slider = null;
    public void SetPercentage(float percentage)
    {
        amount = percentage;
        if (amount < 0) amount = 0;
        if (amount > 1) amount = 1;
    }

    private void Update()
    {
        //update rotation && fill
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(-180 * amount, Vector3.forward), Time.deltaTime);
        m_Slider.fillAmount = amount * 0.5f;

    }
}
