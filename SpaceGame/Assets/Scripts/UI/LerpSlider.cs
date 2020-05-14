using UnityEngine;
using UnityEngine.UI;

public class LerpSlider : MonoBehaviour
{
    private Slider m_slider = null;
    private float m_lerpTime = 1.0f;
    private float m_currentLerpTime = 0;
    private float m_targetSliderValue = 0;
    //the slider should not reach 0 but a value close to 0 so that it does not dissapear
    private float m_minSliderValue = 0;
    //setup slider and lerp speed
    public void Init(Slider slider, float lerpTime = 1.0f, float minValue =0.1f)
    {
        m_slider = slider;
        m_lerpTime = lerpTime;
        m_minSliderValue = minValue;
    }
    //Update slider target value & reset lerp time
    public void UpdateSlider(int value)
    {
        //if slider is empty display min slider value
        if (value == 0) m_targetSliderValue = m_minSliderValue;
        else m_targetSliderValue = value;

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
