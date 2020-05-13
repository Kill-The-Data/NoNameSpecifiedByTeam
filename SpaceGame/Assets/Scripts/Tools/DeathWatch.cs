using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWatch : MonoBehaviour, IObserver
{
    private bool m_hasStarted = false;

    public PlayerHealth PlayerHealth;
    public State State;
    public Timer timer;

    void StartDeathWatch()
    {
        m_hasStarted = true;
    }

    void Update()
    {
        if (PlayerHealth && PlayerHealth.IsDead())
        {
            GameOver();
        }
    }
    private void GameOver()
    {
        if (State is IngameState ingameState) ingameState.PlayerDied();
        else if (State is TutorialState tutorialState) tutorialState.PlayerDied();
    }
    //gets notified by timer
    public void GetUpdate(ISubject subject)
    {
        if (subject is Timer timer)
        {
            if (timer.GetState() == Timer.TimerState.OUT_OF_TIME) 
            {
                GameOver();
            }
        }
    }
}
