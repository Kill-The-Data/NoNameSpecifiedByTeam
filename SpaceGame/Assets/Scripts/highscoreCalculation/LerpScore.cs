using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;

public class LerpScore : MonoBehaviour
{
    public enum CalculationState
    {
        START,
        TIME_ADDED,
        HEALTH_ADDED,
        BOYS_ADDED,
        FINISHED
    }

    [Header("---Setup---")]
    [Header("-text start pos-")]
    [SerializeField] private Vector3 m_startPos =new Vector3(0,-300,0);
    [Header("-tween type-")]
    [Header("---tween setup---")]
    [SerializeField] private LeanTweenType m_type = LeanTweenType.linear;
    [Header("-delay in between tween-")]
    [SerializeField] private float delay = 0.5f;
    [SerializeField] private float m_tweenSpeed = 1.0f;
    [Header("-horizontal distance between text fields type-")]
    [SerializeField] private float m_distance = 50.0f;


    [Header("---Assign Text objects---")]
    [SerializeField] private TMP_Text m_ScoreText = null;
    [SerializeField] private TMP_Text m_timeLeftText = null;
    [SerializeField] private TMP_Text m_HealthLeftText = null;
    [SerializeField] private TMP_Text m_ScoreGainText = null;
    [SerializeField] private TMP_Text m_buoysFilledUp = null;
    [SerializeField] private TMP_Text m_finishedText = null;

    //vars
    private float m_currentScore = 0;
    private float m_timeLeft = 0;
    private int m_healthLeft = 0;
    private int m_filledUp = 0;
    private int m_finished = 0;
    private bool m_delaying = false;

    private CalculationState m_currentState = CalculationState.START;
    private float m_currentT = 0;

    public void Reset()
    {
        ResetValues();
        if (m_ScoreText)
            m_ScoreText.text = m_currentScore.ToString();
    }

    private void ResetValues()
    {
        m_currentState = CalculationState.START;
        m_currentScore = 0;
        m_timeLeft = 0;
        m_healthLeft = 0;
        m_filledUp = 0;
        m_finished = 0;
        m_delaying = false;
    }

    //update handles delay
    void Update()
    {
        if (m_delaying)
        {
            m_currentT -= Time.deltaTime;
            if (m_currentT <= 0)
            {
                m_delaying = false;
                Calculate();
            }
        }
    }
    //start of calculation
    //init text & vars
    public void CalculateScore(int initialScore, float timeLeft, int healthLeft, int filledBoys, int reachedGoal)
    {
        m_currentScore = initialScore;
        m_timeLeft = timeLeft;
        m_healthLeft = healthLeft;
        m_filledUp = filledBoys;
        m_finished = reachedGoal;
        m_ScoreText.text = initialScore.ToString();


        InitText(reachedGoal.ToString(), m_finishedText);
        InitText(filledBoys.ToString(), m_buoysFilledUp);
        InitText(m_timeLeft.ToString(), m_timeLeftText);
        InitText(m_healthLeft.ToString(), m_HealthLeftText);


        AddTime();

    }

    private void InitText(string text, TMP_Text textContainer)
    {
        if (textContainer)
        {
            textContainer.text = text;
            textContainer.transform.parent.transform.localPosition = m_startPos;
            textContainer.transform.parent.gameObject.SetActive(false);
        }
    }
    //chages state, calcs score gain and calls add score 
    private void AddTime()
    {
        m_currentState = CalculationState.TIME_ADDED;

        Debug.Log("time left" + m_timeLeft);
        int addValue = Mathf.RoundToInt(m_timeLeft * 5);
        AddScore(m_timeLeftText, addValue);
    }

    private void AddHealth()
    {
        Debug.Log("health left" + m_healthLeft);

        m_currentState = CalculationState.HEALTH_ADDED;
        int addValue = m_healthLeft * 10;
        AddScore(m_HealthLeftText, addValue);
    }
    private void AddFilledBoys()
    {
        m_currentState = CalculationState.BOYS_ADDED;
        int addValue = m_filledUp * 50;
        AddScore(m_buoysFilledUp, addValue);
    }
    private void AddFinishGoal()
    {
        m_currentState = CalculationState.FINISHED;
       int addvalue = (int)m_currentScore * m_finished;
        AddScore(m_finishedText, addvalue);
    }

    //moves text object
    private void AddScore(TMP_Text text, int scoreAdded)
    {
        LerpMainScore(scoreAdded);
        text?.gameObject.transform.parent.gameObject.SetActive(true);
        ScoreGainDisplay(scoreAdded);
        LeanTween.moveY(text?.gameObject.transform.parent.gameObject, transform.position.y - m_distance * (int)m_currentState, m_tweenSpeed).setEase(m_type);
        m_currentScore += scoreAdded;
    }
    //activates delay, sets score if done
    private void Step()
    {
        if (m_currentState == CalculationState.FINISHED)
        {
            PlayerPrefs.SetInt("score", (int)m_currentScore);
        }
        m_currentT = delay;
        m_delaying = true;
    }
    //do calculation based on current calculation state
    private void Calculate()
    {
        switch (m_currentState)
        {
            case CalculationState.START:
                AddTime();
                break;
            case CalculationState.TIME_ADDED:
                AddHealth();
                break;
            case CalculationState.HEALTH_ADDED:
                AddFilledBoys();
                break;
            case CalculationState.BOYS_ADDED:
                AddFinishGoal();
                break;
        }
    }
    //displays the gained score
    private void ScoreGainDisplay(int gain)
    {
        m_ScoreGainText.text = "+ " + gain.ToString();
        m_ScoreGainText.gameObject.SetActive(true);

        LeanTween.value(this.gameObject, gain, 0, m_tweenSpeed).setEase(m_type).setOnUpdate((float val) =>
        {
            int roundedVal = Mathf.RoundToInt(val);
            m_ScoreGainText.text = "+ " + roundedVal.ToString();
            if (roundedVal == 0) m_ScoreGainText.gameObject.SetActive(false);
        });
    }
    //lerps current score to new score
    private void LerpMainScore(float newTargetValue)
    {
        LeanTween.value(this.gameObject, m_currentScore, m_currentScore + newTargetValue, m_tweenSpeed).
           setEase(m_type).setOnUpdate(UpdateText).setOnComplete(Step);
    }
    private void UpdateText(float f)
    {
        int score = Mathf.RoundToInt(f);
        m_ScoreText.text = score.ToString();
    }

}
