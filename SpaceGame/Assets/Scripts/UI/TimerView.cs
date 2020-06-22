using System;
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
    
    [SerializeField] private bool NoWebConfig = false;
    
    [Tooltip("Initial Amount of Fuel")]
    [SerializeField] private float m_initalAmountOfFuel = 60;
    [SerializeField] private float m_maxAmountOfFuel = 60;

    private Action<Timer> m_OnInitTimerEvent; 
    
    private float time;

    private bool m_timerWasInit = false;
    
    public Timer timer
    {
        get;
        private set;
    } = null;
    private List<IObserver> m_Observers;
    public void InitTimer()
    {
        
        m_timerWasInit = false;
        gameObject.SetActive(true);
        if (!timer)
        {
            timer = gameObject.AddComponent<Timer>();
            //attach self to the newly created timer
            timer.Attach(this);
            //start the timer with the provided duration
        }
        
        SetTimerValues();
        
        if(!NoWebConfig) WebConfigHandler.OnFinishDownload(o =>
        {
            o.ExtractInt("initial_fuel"  , v => m_initalAmountOfFuel=v);
            o.ExtractInt("max_fuel", v => m_maxAmountOfFuel = v);
            SetTimerValues();
        });

        m_OnInitTimerEvent?.Invoke(timer);
        m_timerWasInit = true;
        m_OnInitTimerEvent = null;
    }

    private void SetTimerValues()
    {
        time = m_initalAmountOfFuel;
        timer.Max = m_maxAmountOfFuel;
        timer.StartTimer(time);
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
        
        UpdateText(time);
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
                time = timer.GetTime();
            }
            else if (timer.GetState() == Timer.TimerState.OUT_OF_TIME)
            {
                // when the timer exits set the time one last time to 0
                time = 0;
            }
        }
    }

    //set the text to the remaining time in seconds
    private void UpdateText(float time)
    {
        m_text.SetText(Mathf.RoundToInt(time).ToString() + "L");
        m_rotator.SetPercentage(time / m_maxAmountOfFuel);
    }

    public float GetMaxFuel()
    {
        return m_maxAmountOfFuel;
    }

    public void OnTimerInit(Action<Timer> action)
    {
        if (m_timerWasInit)
        {
            action(timer);
        }

        m_OnInitTimerEvent += action;
    }
}
