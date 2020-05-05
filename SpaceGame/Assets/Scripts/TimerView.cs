using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public interface IObserver
{
     void GetUpdate(ISubject subject);

}
public interface ISubject
{
     void Notify();
     void Attach(IObserver observer);
}


public class TimerView : MonoBehaviour, IObserver
{
    [SerializeField] private TMP_Text _Text;
    [SerializeField] private Timer _timer;
    [SerializeField] private float _StartDuration;
    private void Start()
    {
        if (_timer) 
        {
            _timer.Attach(this);
            _timer.StartTimer(_StartDuration);
        }
    }
    public void GetUpdate()
    {
    }

    public void GetUpdate(ISubject subject)
    {

        if (subject is Timer)
        {
            Timer timer = subject as Timer;
            if (timer.GetState() == Timer.TimerState.ACTIVE) 
            {
                float timeLeft = timer.GetTime();
                UpdateText(timeLeft);
            }
            else if (timer.GetState() == Timer.TimerState.OUT_OF_TIME) 
            {
                UpdateText(0);
            }

        }
    }
    private void UpdateText(float time)
    {
        int timeRounded = Mathf.RoundToInt(time);
        string message = timeRounded.ToString();

        _Text.SetText(message);
    }
}
