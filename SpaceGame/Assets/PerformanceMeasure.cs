using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceMeasure : MonoBehaviour, IObserver
{

    [Header("---Setup---")]
    [SerializeField] private bool m_logPerformance = false;

    [Header("---Current Performance Output---")]
    public float m_timeLeft = 0;
    public float m_timePassed = 0;

    public float m_healthLeft = 0;
    public float m_healthLost = 0;

    public int m_score = 0;

    private void Awake()
    {
        Reset();
    }

    public void Reset()
    {

    }
    void Update()
    {
        m_score = PlayerPrefs.GetInt("score");
        if (m_logPerformance)
            LogPerformance();
    }

    private void LogPerformance()
    {
        Debug.Log("-----------------------------");
        Debug.Log("Current player performance:");
        Debug.Log("Time passed: " + m_timePassed);
        Debug.Log("Time Left: " + m_timeLeft);
        Debug.Log("Health lost: " + m_healthLost);
        Debug.Log("Health left: " + m_healthLeft);
        Debug.Log("Score: " + m_score);
    }

    public void GetUpdate(ISubject subject)
    {
        if (subject is Timer timer)
        {
            m_timeLeft = timer.GetTime();
            m_timePassed = timer.GetStartTime() - timer.GetTime();
            return;
        }
        else if (subject is PlayerHealth health)
        {
            m_healthLeft = health.Health;
            m_healthLost = health.GetMaxHealth() - health.Health;
            return;

        }
    }
}
