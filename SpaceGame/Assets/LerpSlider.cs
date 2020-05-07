using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class LerpSlider : MonoBehaviour
{
    private Slider m_slider = null;
    private float m_lerpTime = 1.0f;
    private float m_currentLerpTime = 0;
    private float m_targetSliderValue = 0;
    //setup slider and lerp speed
    public void Init(Slider slider, float lerpTime = 1.0f)
    {
        m_slider = slider;
        m_lerpTime = lerpTime;
    }
    //Update slider target value & reset lerp time
    public void UpdateSlider(float value)
    {
        m_targetSliderValue = value;
        m_currentLerpTime = 0;
    }
    private void Update()
    {
        //check if target value 
        if (m_slider.value != m_targetSliderValue)
        {
            //update time 
            m_currentLerpTime += Time.deltaTime;
            if (m_currentLerpTime > m_lerpTime)
            {
                m_currentLerpTime = m_lerpTime;
            }
            //lerp & update slider value
            float perc = m_currentLerpTime / m_lerpTime;
            m_slider.value = LeanTween.easeOutExpo(m_slider.value, m_targetSliderValue, perc);
        }
    }
}
