using UnityEngine;

public class MotherShipCollisionHandler : MonoBehaviour
{
    [Header(" --- Setup ---")]
    [SerializeField] private ScoreUI m_scoreUI;
    [SerializeField] private FSM m_Fsm;

    [Header(" --- Hack Refactor Me ---")] 
    [SerializeField] private bool m_ignoreFsm = false;
    [SerializeField] private bool m_ignoreScoreUI = false;
    
    
    [Header(" --- Gameplay ---")]
    [Tooltip("How much the player gets for one piece of cargo")]
    [SerializeField] private int m_scorePerCargo = 10;

    private BuoyFillUp m_FillUp = null;

    void Start()
    {
        m_FillUp = GetComponent<BuoyFillUp>();
    }
    public void OnTriggerEnter(Collider other)
    {
        //check if the Trigger Participant is the Player and if he has a PlayerCargo Component 
        if (other.CompareTag("Player") 
            && other.transform.parent.GetComponentSafe<PlayerCargo>(out var cargo))
        {
            //add score to the 
            int cargoAmount = cargo.SpaceOccupied;

            int leftOverCargo = m_FillUp.DropOff(cargoAmount);

            
            //TODO(anyone): Fix these hacks with an observer pattern or something please
            
            if(m_ignoreScoreUI){
                int scoreGain = ((cargoAmount - leftOverCargo) * m_scorePerCargo);
                if (scoreGain != 0) m_scoreUI.AddScore(scoreGain);
                cargo.SetFill(leftOverCargo);
            }
            
            if (!m_ignoreFsm && m_Fsm.GetCurrentState() is TutorialState currentState)
            {
                currentState.FinishTutorial();
            }
        }
    }
}
