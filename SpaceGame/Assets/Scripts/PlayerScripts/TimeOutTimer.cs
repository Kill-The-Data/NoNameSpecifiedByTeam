using UnityEngine;

public class TimeOutTimer : MonoBehaviour, IObserver
{

    private const float m_TimeOutDuration = 30.0f;
    private State m_ingameState = null;
    private Timer _timer;

    public bool Disabled = false;
    
    public void InitTimer(State currentState)
    {
        m_ingameState = currentState;
        _timer = gameObject.GetComponent<Timer>();
        if (_timer) ResetTimer();
        else _timer = gameObject.AddComponent<Timer>();

        if (_timer)
        {
            //attach self to the newly created timer
            _timer.Attach(this);
            //start the timer with the provided duration
            _timer.StartTimer(m_TimeOutDuration+5);
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
        _timer?.StartTimer(m_TimeOutDuration);
    }
    public void GetUpdate(ISubject subject)
    {
        if (subject is Timer timer)
        {

            if (!Disabled && timer.GetState() == Timer.TimerState.OUT_OF_TIME)
            {
                // when the timer exits set the time one last time to 0
                if (m_ingameState is IngameState state) state.TimeOut();
                else if (m_ingameState is TutorialState otherState) otherState.TimeOut();
            }
        }
    }
}
