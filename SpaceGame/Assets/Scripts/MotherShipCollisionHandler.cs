using UnityEngine;

public class MotherShipCollisionHandler : MonoBehaviour
{


    [Header(" --- Gameplay ---")]
    [Tooltip("How much the player gets for one piece of cargo")]
    [SerializeField] private int m_scorePerCargo = 10;

    private ScoreUI m_scoreUI;
    private FSM m_Fsm;
    private BuoyFillUp m_FillUp = null;

    void Start()
    {
        FindTaggedObjects();
        m_FillUp = GetComponent<BuoyFillUp>();
    }

    //trys to find the object by its tag, please do not reuse the Tag, tag should be unique for this
    private void FindTaggedObjects()
    {
        if (!m_scoreUI)
            m_scoreUI = GameObject.FindWithTag("ScoreUI").GetComponent<ScoreUI>();
        if (!m_Fsm)
            m_Fsm = GameObject.FindWithTag("FSM").GetComponent<FSM>();

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
            int scoreGain = ((cargoAmount - leftOverCargo) * m_scorePerCargo);
            if (scoreGain != 0) m_scoreUI.AddScore(scoreGain);
            cargo.SetFill(leftOverCargo);

            if (m_Fsm.GetCurrentState() is TutorialState currentState)
            {
                currentState.FinishTutorial();
            }
        }
    }
}
