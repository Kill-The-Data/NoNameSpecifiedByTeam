using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOutTimer : MonoBehaviour, IObserver
{

     private const float m_TimeOutDuration = 5.0f;
    [SerializeField] private IngameState ingameState = null;
    private Timer _timer;
    public void InitTimer()
    {
        _timer = gameObject.AddComponent<Timer>();

        if (_timer)
        {
            //attach self to the newly created timer
            _timer.Attach(this);
            //start the timer with the provided duration
            _timer.StartTimer(m_TimeOutDuration);
        }
    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            ResetTimer();
        }
    }
    private void ResetTimer()
    {
        _timer.StartTimer(m_TimeOutDuration);
    }
    public void GetUpdate(ISubject subject)
    {
        if (subject is Timer timer)
        {

            if (timer.GetState() == Timer.TimerState.OUT_OF_TIME)
            {
                // when the timer exits set the time one last time to 0
                Debug.Log("return to menu");
                ingameState.TimOut();
            }
        }
    }
}
