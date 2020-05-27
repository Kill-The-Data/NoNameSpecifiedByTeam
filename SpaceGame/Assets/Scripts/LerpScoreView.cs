using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpScoreView : MonoBehaviour
{
    [SerializeField] private float m_TweenSpeed = 1.0f;
    [SerializeField] private float m_DisplayDuration = 5.0f;
    [SerializeField] private GameObject m_firstScoreDisplay = null;
    [SerializeField] private GameObject m_secondScoreDisplay = null;
    [SerializeField] private LeanTweenType m_tweenType = LeanTweenType.linear;

    private float m_hiddenScoreY = 0;
    private float m_targetScoreY = 0;
    [SerializeField] private float m_targetInivsY = 500;

    private float m_TimeLeft = 0;
    private bool m_firstIsVisible = true;
    void Start()
    {
        m_hiddenScoreY = m_secondScoreDisplay.transform.localPosition.y;
        m_targetScoreY = m_firstScoreDisplay.transform.localPosition.y;
        reset();
    }

    void Update()
    {

        m_TimeLeft -= Time.deltaTime;
        if (m_t <= 0)
        {
            SwitchView();
            Reset();
        }
    }

    private void SwitchView()
    {
        if (m_firstIsVisible)
        {
            m_secondScoreDisplay.LeanMoveLocalY(m_targetScoreY, m_TweenSpeed).setEase(m_tweenType);
            m_firstScoreDisplay.LeanMoveLocalY(m_targetInivsY, m_TweenSpeed).setEase(m_tweenType).setOnComplete(MoveBack);
            m_firstIsVisible = false;
        }
        else
        {
            m_firstScoreDisplay.LeanMoveLocalY(m_targetScoreY, m_TweenSpeed).setEase(m_tweenType).setOnComplete(MoveBack);
            m_secondScoreDisplay.LeanMoveLocalY(m_targetInivsY, m_TweenSpeed).setEase(m_tweenType);
            m_firstIsVisible = true;
        }
    }

    private void MoveBack()
    {
        if (m_firstIsVisible)
        {
            Vector3 newPos = m_secondScoreDisplay.transform.localPosition;
            newPos.y = m_hiddenScoreY;
            m_secondScoreDisplay.transform.localPosition = newPos;
        }
        else
        {
            Vector3 newPos = m_firstScoreDisplay.transform.localPosition;
            newPos.y = m_hiddenScoreY;
            m_firstScoreDisplay.transform.localPosition = newPos;
        }
    }
    void Reset()
    {
        m_TimeLeft = m_DisplayDuration;
    }
}
