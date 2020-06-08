using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerView : MonoBehaviour, IObserver
{

    [Header(" --- Setup ---")]
    [Tooltip("The Text to display the remaining time on")]
    [SerializeField] private TMP_Text m_text = null;

    [Tooltip("The Needle of the Fuel Gauge")]
    [SerializeField] private FuelGaugeRotator m_rotator;

    [Tooltip("How long the timer should tick for")]
    [SerializeField] private float m_duration = 30F;

    private const float INITIAL_AMOUNT_OF_FUEL = 60;
    
    public Timer timer
    {
        get;
        private set;
    } = null;
    private List<IObserver> m_Observers;
    public void InitTimer()
    {
        gameObject.SetActive(true);
        if (!timer)
        {
            timer = gameObject.AddComponent<Timer>();
            //attach self to the newly created timer
            timer.Attach(this);
            //start the timer with the provided duration
        }
        timer.StartTimer(m_duration);
    }
    public void Update()
    {
        if (Input.GetMouseButton(0))
        {
            timer?.Continue();
        }
        else
        {
            timer?.Pause();
        }
    }
    public void AttachPerformanceMeasure(PerformanceMeasure performance)
    {
        timer.Attach(performance);
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
        m_rotator.SetPercentage(time / INITIAL_AMOUNT_OF_FUEL);
    }
}
