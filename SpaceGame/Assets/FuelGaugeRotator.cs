using System;
using UnityEngine;
using UnityEngine.UI;

public class FuelGaugeRotator : MonoBehaviour
{
    //fill factor
    private float amount = 1;
    //radial slider image reference
    [SerializeField] private Image m_Slider = null;
    //fill value for radial fill
    private float m_radialFillAmount = 0.5f;
    public void SetPercentage(float percentage)
    {
        amount = percentage;
        if (amount < 0) amount = 0;
        if (amount > 1) amount = 1;
        //fill is halved for radial fill -> fill for circle: max is 0.5f min is 0
        m_radialFillAmount = amount * 0.5f;

    }

    private void Update()
    {
        //update rotation && fill
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(-180 * amount, Vector3.forward), Time.deltaTime);
        m_Slider.fillAmount = m_radialFillAmount;

    }
}
