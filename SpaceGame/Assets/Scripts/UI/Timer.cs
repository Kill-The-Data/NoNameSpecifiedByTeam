using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour, ISubject
{
    public enum TimerState
    {
        ACTIVE,
        OUT_OF_TIME
    }
    private TimerState m_state = TimerState.OUT_OF_TIME;
    private List<IObserver> m_Observers = new List<IObserver>();
    private float m_TimeLeft = 0;
    private float m_StartTime = 0;
    void Update()
    {
        if (m_state == TimerState.ACTIVE)
        {
            m_TimeLeft -= Time.deltaTime;

            if (m_TimeLeft <= 0)
            {
                m_state = TimerState.OUT_OF_TIME;
            }
            Notify();
        }
    }

    public float GetStartTime() => m_StartTime;
    public TimerState GetState() => m_state;
    public float GetTime() => m_TimeLeft;

    public void StartTimer(float newDuration)
    {
        m_StartTime = newDuration;
        m_TimeLeft = newDuration;
        m_state = TimerState.ACTIVE;
    }

    public void Notify()
    {
        foreach (IObserver observer in m_Observers)
        {
            observer.GetUpdate(this);
        }
    }

    public void Attach(IObserver observer)
    {
        m_Observers.Add(observer);
    }
}
