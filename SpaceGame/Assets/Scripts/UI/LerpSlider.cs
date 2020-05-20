using UnityEngine;
using UnityEngine.UI;

public class LerpSlider : MonoBehaviour, IObserver
{
    //target fill up
    private Slider m_slider = null;
    private Material m_targetFillUpRadial = null;

    //lerp vars
    private float m_lerpTime = 1.0f;
    private float m_currentLerpTime = 0;
    private float m_targetSliderValue = 0;
    //the slider should not reach 0 but a value close to 0 so that it does not dissapear
    private float m_minSliderValue = 0;

    private float m_LastFIll = 0;
    //defines Fill type
    private bool m_IsRadialFillUp;
    //init for fill up bar
    public void Init(Slider slider, float lerpTime = 1.0f, float minValue = 0.1f)
    {
        m_slider = slider;
        m_lerpTime = lerpTime;
        m_minSliderValue = minValue;
        m_IsRadialFillUp = false;

    }
    //init for radial fill up
    public void Init(Material mat, float lerpTime = 1.0f, float minValue = 0.1f)
    {
        m_LastFIll = 0;
        m_targetFillUpRadial = mat;
        m_lerpTime = lerpTime;
        m_minSliderValue = minValue;
        m_IsRadialFillUp = true;
    }
    //Update slider target value & reset lerp time
    public void UpdateSlider(float value)
    {
        //if slider is empty display min slider value
        if (value == 0) m_targetSliderValue = m_minSliderValue;
        else m_targetSliderValue = value;
        m_currentLerpTime = 0;
    }

    private float Lerp(float currentValue)
    {
        //update time 
        m_currentLerpTime += Time.deltaTime;
        if (m_currentLerpTime > m_lerpTime)
        {
            m_currentLerpTime = m_lerpTime;
        }
        //lerp fill value
        float perc = m_currentLerpTime / m_lerpTime;
        float fillUpValue = LeanTween.easeOutExpo(currentValue, m_targetSliderValue, perc);
        return fillUpValue;
    }
    private void Update()
    {
        //fill up target if target value not reached
        if (m_IsRadialFillUp && m_targetFillUpRadial != null)
        {
            float fill = Lerp(m_LastFIll);
            m_targetFillUpRadial.SetFloat("FILL", fill);
            m_LastFIll = fill;
        }
        else if (m_slider)
        {
            if (m_slider.value != m_targetSliderValue)
                m_slider.value = Lerp(m_slider.value);
        }

    }

    public void GetUpdate(ISubject subject)
    {
        if (subject is PlayerHealth health)
        {
            UpdateSlider(health.Health);
        }
    }
}

