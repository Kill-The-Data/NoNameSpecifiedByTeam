using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishCollisionHandler : MonoBehaviour
{

    private FSM m_FSM =null;

    private void Start()
    {
        m_FSM = GameObject.FindGameObjectWithTag("FSM")?.GetComponent<FSM>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FinishGame();
        }
    }

    private void FinishGame()
    {
        Debug.Log("Finishing Game");
        if (m_FSM)
        {
            State currentState = m_FSM.GetCurrentState();
            if(currentState is IngameState ingameState)
                ingameState.GameFinished();
        }
    }


}
