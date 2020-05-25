using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceMeasure : MonoBehaviour, IObserver
{
    public enum Difficulty
    {
        NOOB,
        EASY,
        NORMAL,
        HARD,
        DEATH_MARCH
    }

    [Header("---Setup---")]
    [SerializeField] private Difficulty m_currentDifficulty = Difficulty.NORMAL;
    [SerializeField] private float m_difficultyUpdateTimeStamp = 10.0f;
    //curently uses dmg taken over intervall as mesurement
    [Tooltip("curently uses dmg taken over intervall as mesurement")]
    [SerializeField] private float m_difficultyIncreaseThreshold = 0.5f;
    [SerializeField] private float m_difficultyDecreaseThreshold = 1.5f;


    [Header("---Current Performance Output---")]
    [SerializeField] private float m_timeLeft = 0;
    [SerializeField] private float m_timePassed = 0;
    [SerializeField] private int m_healthLeft = 0;
    [SerializeField] private float m_damageTaken = 0;
    [SerializeField] private float m_damageOverTimer = 0;
    [SerializeField] private float m_dmgOverIntervall = 0;
    [SerializeField] private float m_TimeWithoutDamage = 0;
    [SerializeField] private int m_score = 0;
    [SerializeField] private float m_DmgTakenSinceLastUpdate = 0;
    private int m_lastUpdateScore = 0;

    private float m_currentTime = 0;


    public Difficulty GetDifficulty() => m_currentDifficulty;
    private void Awake()
    {
        Reset();
    }

    public void Reset()
    {
        m_healthLeft = 0;
        ResetTimer();
    }

    public void StoreStatsInPlayerPrefs(int lvlCompleted)
    {
        PlayerPrefs.SetInt("score", m_score);
        PlayerPrefs.SetFloat("time", m_timeLeft);
        PlayerPrefs.SetInt("health", m_healthLeft); 
        PlayerPrefs.SetInt("goalReached", lvlCompleted);
    }
    void Update()
    {
        m_score = PlayerPrefs.GetInt("score");
        UpdateTimeStamp();
    }
    
    private void ResetTimer()
    {
        m_currentTime = 0;
    }
    private void UpdateTimeStamp()
    {
        m_TimeWithoutDamage += Time.deltaTime;
        m_currentTime += Time.deltaTime;
        m_dmgOverIntervall = m_DmgTakenSinceLastUpdate / m_currentTime;
        if (m_currentTime >= m_difficultyUpdateTimeStamp)
        {
            ResetTimer();
            UpdateDifficulty();
        }
    }
    private void UpdateDifficulty()
    {
        m_DmgTakenSinceLastUpdate = 0;
        if (m_damageOverTimer > m_difficultyDecreaseThreshold)
        {
            DecreaseDifficulty();
        }
        else if (m_dmgOverIntervall < m_difficultyIncreaseThreshold)
        {
            IncreaseDifficulty();
        }

    }
    private void IncreaseDifficulty()
    {
        switch (m_currentDifficulty)
        {
            case Difficulty.NOOB:
                m_currentDifficulty = Difficulty.EASY;
                break;
            case Difficulty.EASY:
                m_currentDifficulty = Difficulty.NORMAL;
                break;
            case Difficulty.NORMAL:
                m_currentDifficulty = Difficulty.HARD;
                break;
            case Difficulty.HARD:
                m_currentDifficulty = Difficulty.DEATH_MARCH;
                break;
        }
    }

    private void DecreaseDifficulty()
    {
        switch (m_currentDifficulty)
        {
            case Difficulty.EASY:
                m_currentDifficulty = Difficulty.NOOB;
                break;
            case Difficulty.NORMAL:
                m_currentDifficulty = Difficulty.EASY;
                break;
            case Difficulty.HARD:
                m_currentDifficulty = Difficulty.NORMAL;
                break;
            case Difficulty.DEATH_MARCH:
                m_currentDifficulty = Difficulty.HARD;
                break;
        }
    }
    //process data 
    public void GetUpdate(ISubject subject)
    {
        if (subject is Timer timer)
        {
            m_timeLeft = timer.GetTime();
            m_timePassed = timer.GetStartTime() - timer.GetTime();

            m_damageOverTimer = m_damageTaken / m_timePassed;
            return;
        }
        else if (subject is PlayerHealth health)
        {
            HandleDmg(health);
            return;
        }
    }

    private void HandleDmg(PlayerHealth health)
    {
        m_damageTaken = health.GetMaxHealth() - health.Health;
        //if initial dmg take the new dmg
        if (m_healthLeft == 0)
        {
            m_DmgTakenSinceLastUpdate += m_damageTaken;
        }//else add the delta dmg
        else
        {
            //add delta dmg
            m_DmgTakenSinceLastUpdate += (m_healthLeft - health.Health);
        }
        m_healthLeft = health.Health;
        m_TimeWithoutDamage = 0;
    }
}
