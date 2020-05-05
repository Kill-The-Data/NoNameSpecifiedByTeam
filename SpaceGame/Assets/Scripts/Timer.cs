using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Timer : MonoBehaviour, ISubject
{
    public enum TimerState
    {
        ACTIVE,
        OUT_OF_TIME,
        NOT_ACTIVE,
    }
    private TimerState _state = TimerState.NOT_ACTIVE;
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
                Notify();
            }
            else
            {
                Notify();
            }
        }
    }
    public TimerState GetState() { return _state; }
    public float GetTime() { return _TimeLeft; }

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
