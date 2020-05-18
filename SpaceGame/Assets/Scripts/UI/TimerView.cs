﻿using Boo.Lang;
using UnityEngine;
using TMPro;

public class TimerView : MonoBehaviour, IObserver
{

    [Header(" --- Setup ---")]
    [Tooltip("The Text to display the remaining time on")]
    [SerializeField] private TMP_Text m_text = null;

    [Tooltip("How long the timer should tick for")]
    [SerializeField] private float m_duration = 30F;

    private Timer _timer;
    private List<IObserver> m_Observers;
    public void InitTimer()
    {
        gameObject.SetActive(true);
        if (!_timer)
        {
            _timer = gameObject.AddComponent<Timer>();
            //attach self to the newly created timer
            _timer.Attach(this);
            //start the timer with the provided duration
        }
        _timer.StartTimer(m_duration);
    }

    public void AttachPerformanceMeasure(PerformanceMeasure performance)
    {
        _timer.Attach(performance);
    }
    public void GetUpdate(ISubject subject)
    {

        if (subject is Timer timer)
        {
            //check if the timer is still active
            if (timer.GetState() == Timer.TimerState.ACTIVE)
            {
                // update the text
                UpdateText(timer.GetTime());
            }
            else if (timer.GetState() == Timer.TimerState.OUT_OF_TIME)
            {
                // when the timer exits set the time one last time to 0
                UpdateText(0);
            }
        }
    }

    //set the text to the remaining time in seconds
    private void UpdateText(float time)
    {
        m_text.SetText(Mathf.RoundToInt(time).ToString() + "s");
    }
}
