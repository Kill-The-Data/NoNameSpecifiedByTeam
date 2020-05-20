using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyFillUp : MonoBehaviour
{

    [Header(" --- Boy values setup ---")]
    [SerializeField] private int m_MaxFillUp;

    [Header(" --- UI setup ---")]
    [Range(0, 1.5f)]
    [SerializeField] private float m_tweenSpeed = 1.0f;
    [Tooltip("The slider should never reach 0, only close to 0 so that it does not disappear")]
    [Range(0.01f, 0.2f)]
    [SerializeField] private float m_MinSliderValue = 0.1f;

    //[SerializeField] private GameObject m_TargetRadialFillUp = null;
    [SerializeField] private SpriteRenderer m_TargetRenderer = null;

    private int m_currentFillUp;
    private LerpSlider m_Lerper;

    void Start()
    {
        Init();
    }
    public void Init()
    {
        if (!m_Lerper)
        {
            m_Lerper = m_TargetRenderer.gameObject.AddComponent<LerpSlider>();
            Material mat = m_TargetRenderer.material;
            m_Lerper.Init(mat, m_tweenSpeed, m_MinSliderValue);
        }
        m_Lerper.UpdateSlider(0);
        m_currentFillUp = 0;
    }


    public int DropOff(int dropOffAmount)
    {
        int remainingCargo = 0;

        // add cargo
        m_currentFillUp += dropOffAmount;

        //set fill up to max if it exceeds max
        if (m_currentFillUp > m_MaxFillUp)
        {
            remainingCargo = m_currentFillUp - m_MaxFillUp;
            m_currentFillUp = m_MaxFillUp;
        }
        //return leftover player cargo if not all cargo can be dropped off
        float fillUp = (float)m_currentFillUp / (float)m_MaxFillUp;
        m_Lerper.UpdateSlider(fillUp + 0.05f);
        return remainingCargo;

    }

    public bool Full()
    {
        return m_currentFillUp >= m_MaxFillUp;
    }
}
