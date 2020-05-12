using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWatch : MonoBehaviour
{
    private bool m_hasStarted = false;

    public PlayerHealth PlayerHealth;
    public State State;    
    
    void StartDeathWatch()
    {
        m_hasStarted = true;
    }

    void Update()
    {
        if (PlayerHealth && PlayerHealth.IsDead() )
        {
            if(State is IngameState ingameState)  ingameState.PlayerDied();
            else if(State is TutorialState tutorialState) tutorialState.PlayerDied();
        }
    }
}
