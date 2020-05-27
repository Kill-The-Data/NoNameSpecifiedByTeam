using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lerpScoreView : MonoBehaviour
{
    [SerializeField] private float m_TweenSpeed = 1.0f;
    [SerializeField] private float m_DisplayDuration = 5.0f;
    [SerializeField] private GameObject firstScoreDisplay = null;
    [SerializeField] private GameObject secondScoreDisplay = null;
    [SerializeField] private LeanTweenType tweenType = LeanTweenType.linear;

    private float m_hiddenScoreY = 0;
    private float m_targetScoreY = 0;
    [SerializeField] private float m_targetInivsY = 500;

    private float m_t = 0;
    private bool m_firstIsVisible = true;
    void Start()
    {
        m_hiddenScoreY = secondScoreDisplay.transform.localPosition.y;
        m_targetScoreY = firstScoreDisplay.transform.localPosition.y;
        reset();
    }

    void Update()
    {
        Debug.Log(m_t);

        m_t -= Time.deltaTime;
        if (m_t <= 0)
        {
            SwitchView();
            reset();
        }
    }

    private void SwitchView()
    {
        if (m_firstIsVisible)
        {
            secondScoreDisplay.LeanMoveLocalY(m_targetScoreY, m_TweenSpeed).setEase(tweenType);
            firstScoreDisplay.LeanMoveLocalY(m_targetInivsY, m_TweenSpeed).setEase(tweenType).setOnComplete(MoveBack);
            m_firstIsVisible = false;
        }
        else
        {
            firstScoreDisplay.LeanMoveLocalY(m_targetScoreY, m_TweenSpeed).setEase(tweenType).setOnComplete(MoveBack);
            secondScoreDisplay.LeanMoveLocalY(m_targetInivsY, m_TweenSpeed).setEase(tweenType);
            m_firstIsVisible = true;

        }

    }

    private void MoveBack()
    {
        if (!m_firstIsVisible)
        {
            Vector3 newPos = firstScoreDisplay.transform.localPosition;
            newPos.y = m_hiddenScoreY;
            firstScoreDisplay.transform.localPosition = newPos;
        }
        else
        {
            Vector3 newPos = secondScoreDisplay.transform.localPosition;
            newPos.y = m_hiddenScoreY;
            secondScoreDisplay.transform.localPosition = newPos;
        }
    }
    void reset()
    {
        m_t = m_DisplayDuration;
    }
}
