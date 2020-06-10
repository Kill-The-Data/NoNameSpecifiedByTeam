using System;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class BlinkyLight : MonoBehaviour
{
    public enum BlinkState
    {
        OFF,
        BLINKING,
        ON
    }

    [Serializable]
    class BlinkStateAndColor
    {
        public BlinkState BlinkState;
        public Color Color;
    }

    [Tooltip("The Inventory of the Buoy")] [LabelOverride("Buoy Inventory")] [SerializeField]
    private BuoyFillUp m_fillUp;

    [SerializeField] private BlinkStateAndColor emptyState;
    [SerializeField] private BlinkStateAndColor partiallyFilledState;
    [SerializeField] private BlinkStateAndColor filledState;
    [SerializeField] private float m_onIntensity = 1;
    [SerializeField] private float m_blinkPeriod = 1;
    [Range(0,1)]
    [SerializeField] private float m_onTime = 0.5f;
    
    
    private bool m_isBlinking = false;
    private Light m_blinkyLight;

    public void Awake()
    {
        m_blinkyLight = GetComponent<Light>();
    }

    private void Blink() => m_isBlinking = true;
    private void UnBlink() => m_isBlinking = false;
    private void Off()=> m_blinkyLight.intensity = 0;
    private void On() => m_blinkyLight.intensity = m_onIntensity;


    private float m_time;

    private void Update()
    {
        CheckCargo();
        BlinkLoop();
    }

    private void CheckCargo()
    {
        switch (m_fillUp.GetState())
        {
            case BuoyFillUp.BuoyCargoState.FULL:
                m_blinkyLight.color = filledState.Color;
                UpdateBlinkState(filledState.BlinkState);
                break;
            case BuoyFillUp.BuoyCargoState.PARTIAL:
                m_blinkyLight.color = partiallyFilledState.Color;
                UpdateBlinkState(partiallyFilledState.BlinkState);
                break;
            case BuoyFillUp.BuoyCargoState.EMPTY:
                m_blinkyLight.color = emptyState.Color;
                UpdateBlinkState(emptyState.BlinkState);
                break;
        }
    }


    private void BlinkLoop()
    {
        m_time += Time.deltaTime;

        if (m_isBlinking)
        {
            if (m_time < m_blinkPeriod * m_onTime) Off();
            if (m_time > m_blinkPeriod * m_onTime) On();

            if (m_time >  m_blinkPeriod)
            {
                m_time = 0;
            }
        }
    }
    
    private void UpdateBlinkState(BlinkState state)
    {
        switch (state)
        {
            case BlinkState.OFF:
                UnBlink();
                
                break;
            case BlinkState.BLINKING:
                Blink();
                break;
            case BlinkState.ON:
                UnBlink();
               
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}