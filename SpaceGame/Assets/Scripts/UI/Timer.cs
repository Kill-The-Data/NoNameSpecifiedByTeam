using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour, ISubject
{
    public enum TimerState
    {
        ACTIVE,
        OUT_OF_TIME
    }
    private TimerState _state = TimerState.OUT_OF_TIME;
    private List<IObserver> _Observers = new List<IObserver>();
    private float _TimeLeft = 0;

    void Update()
    {
        if (_state == TimerState.ACTIVE)
        {
            _TimeLeft -= Time.deltaTime;

            if (_TimeLeft <= 0)
            {
                _state = TimerState.OUT_OF_TIME;
            }
            Notify();
        }
    }
    public TimerState GetState() => _state;
    public float GetTime() => _TimeLeft;

    public void StartTimer(float newDuration)
    {
        _TimeLeft = newDuration;
        _state = TimerState.ACTIVE;
    }

    public void Notify()
    {
        foreach (IObserver observer in _Observers)
        {
            observer.GetUpdate(this);
        }
    }

    public void Attach(IObserver observer)
    {
        _Observers.Add(observer);
    }
}
