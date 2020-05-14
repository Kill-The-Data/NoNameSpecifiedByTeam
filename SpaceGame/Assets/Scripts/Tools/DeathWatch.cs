using UnityEngine;

public class DeathWatch : MonoBehaviour, IObserver
{
    [Tooltip("Reference to the Player Health System")]
    public PlayerHealth PlayerHealth;
    
    [Tooltip("Reference to the currently active State")]
    public State State;

    void Update()
    {
        if (PlayerHealth && PlayerHealth.IsDead())
        {
            GameOver();
        }
    }

    //gets notified by timer
    public void GetUpdate(ISubject subject)
    {
        if (subject is Timer t && t.GetState() == Timer.TimerState.OUT_OF_TIME)
        {
            GameOver();
        }
    }
    
    private void GameOver()
    {
        if (State is IngameState ingameState) ingameState.PlayerDied();
        else if (State is TutorialState tutorialState) tutorialState.PlayerDied();
    }
}
