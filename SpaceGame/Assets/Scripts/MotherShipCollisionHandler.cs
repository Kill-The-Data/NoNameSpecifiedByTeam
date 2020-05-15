using UnityEngine;

public class MotherShipCollisionHandler : MonoBehaviour
{
    [Header(" --- Setup ---")]
    [SerializeField] private ScoreUI m_scoreUI;
    [SerializeField] private FSM m_Fsm;
    [Header(" --- Gameplay ---")]
    [Tooltip("How much the player gets for one piece of cargo")]
    [SerializeField] private int m_scorePerCargo = 10;
    public void OnTriggerEnter(Collider other)
    {
        //check if the Trigger Participant is the Player and if he has a PlayerCargo Component 
        if (other.CompareTag("Player") 
            && other.transform.parent.GetComponentSafe<PlayerCargo>(out var cargo))
        {
            //add score to the 
            int cargoAmount = cargo.SpaceOccupied;
            m_scoreUI.AddScore(cargoAmount * m_scorePerCargo);
            //clear all cargo
            cargo.ClearCargo();
            
            if(m_Fsm.GetCurrentState() is TutorialState currentState) 
            {
                currentState.FinishTutorial();
            }
        }
    }
}
