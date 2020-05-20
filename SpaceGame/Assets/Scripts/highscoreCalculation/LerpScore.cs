using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
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
    public  Vector3 startPos=Vector3.zero;
    private CalculationState m_currentState = CalculationState.START;
    public LeanTweenType type = LeanTweenType.linear;

    public float distance = 50.0f;

    public TMP_Text ScoreText = null; public TMP_Text timeLeftText = null;
    public TMP_Text HealthLeftText = null;
    public TMP_Text ScoreGainText = null;
    public TMP_Text buoysFilledUp = null;
    public TMP_Text finishedText = null;


    public float delay = 0.5f;
    private float currentT = 0;
    public float tweenSpeed = 1.0f;
    // Start is called before the first frame update


    private float m_currentScore = 0;
    private float m_timeLeft = 0;
    private int m_healthLeft = 0;
    private int m_filledUp = 0;
    private bool finished = false;
    private bool delaying = false;
    void Start()
    {
        Reset();
        m_currentScore = 100;
    }

    private void Reset()
    {
        ResetValues();
        m_currentState = CalculationState.START;
        if (!ScoreText) ScoreText = GetComponent<TMP_Text>();
        ScoreText.text = m_currentScore.ToString();
    }

    private void ResetValues()
    {
        m_currentScore = 0;
        m_timeLeft = 0;
        m_healthLeft = 0;
        m_filledUp = 0;
        finished = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            ResetValues();
            CalculateScore(100, 30, 50, 3, true);
        }
            

        if (delaying)
        {
            currentT -= Time.deltaTime;
            if (currentT <= 0)
            {
                delaying = false;
                Calculate();
            }
        }
    }

    public void CalculateScore(int initialScore, float timeLeft, int healthLeft, int filledBoys, bool reachedGoal)
    {
        m_currentScore = initialScore;
        m_timeLeft = timeLeft;
        m_healthLeft = healthLeft;
        m_filledUp = filledBoys;
        finished = reachedGoal;

        ScoreText.text = initialScore.ToString();


        finishedText.text = reachedGoal.ToString();
        finishedText.transform.localPosition = startPos;
        finishedText.gameObject.SetActive(false);

        buoysFilledUp.text = filledBoys.ToString();
        buoysFilledUp.transform.localPosition = startPos;
        buoysFilledUp.gameObject.SetActive(false);

        timeLeftText.text = m_timeLeft.ToString();
        timeLeftText.transform.localPosition = startPos;
        timeLeftText.gameObject.SetActive(false);

        HealthLeftText.text = m_healthLeft.ToString();
        HealthLeftText.transform.localPosition = startPos;
        HealthLeftText.gameObject.SetActive(false);

        AddTime();

    }

    private void AddTime()
    {
        m_currentState = CalculationState.TIME_ADDED;
        int addValue = Mathf.RoundToInt(m_timeLeft * 5);
        AddScore(timeLeftText, addValue);
    }

    private void AddHealth()
    {
        m_currentState = CalculationState.HEALTH_ADDED;
        int addValue = m_healthLeft * 10;
        AddScore(HealthLeftText, addValue);
    }
    private void AddFilledBoys()
    {
        m_currentState = CalculationState.BOYS_ADDED;
        int addValue = m_filledUp * 50;
        AddScore(buoysFilledUp, addValue);
    }
    private void AddFinishGoal()
    {
        m_currentState = CalculationState.FINISHED;
        int addvalue = 0;
        if (finished) addvalue = (int)m_currentScore;
        AddScore(finishedText, addvalue);
    }
    private void AddScore(TMP_Text text, int scoreAdded)
    {
        LerpMainScore(scoreAdded);
        text.gameObject.SetActive(true);
        ScoreGainDisplay(scoreAdded);
        LeanTween.moveY(text.gameObject, transform.position.y - distance * (int)m_currentState, tweenSpeed).setEase(type);
        m_currentScore += scoreAdded;
    }
    private void Step()
    {
        currentT = delay;
        delaying = true;
    }

    private void Calculate()
    {
        switch (m_currentState)
        {
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

    private void ScoreGainDisplay(int gain)
    {
        ScoreGainText.text = "+ " + gain.ToString();
        ScoreGainText.gameObject.SetActive(true);

        LeanTween.value(this.gameObject, gain, 0, tweenSpeed).setEase(type).setOnUpdate((float val) =>
        {
            int roundedVal = Mathf.RoundToInt(val);
            ScoreGainText.text = "+ " + roundedVal.ToString();
            if (roundedVal == 0) ScoreGainText.gameObject.SetActive(false);
        });
    }
    private void LerpMainScore(float newTargetValue)
    {
        LeanTween.value(this.gameObject, m_currentScore, m_currentScore + newTargetValue, tweenSpeed).
           setEase(type).setOnUpdate(UpdateText).setOnComplete(Step);
    }
    private void UpdateText(float f)
    {
        int score = Mathf.RoundToInt(f);
        ScoreText.text = score.ToString();
    }

}
