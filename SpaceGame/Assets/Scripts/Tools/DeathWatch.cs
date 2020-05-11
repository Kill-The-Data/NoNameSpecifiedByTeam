using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWatch : MonoBehaviour
{
    private bool m_hasStarted = false;

    public PlayerHealth PlayerHealth;
    public IngameState IngameState;    
    
    // Start is called before the first frame update
    void StartDeathWatch()
    {
        m_hasStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerHealth && PlayerHealth.IsDead())
        {
            IngameState.PlayerDied();
        }
    }
}
