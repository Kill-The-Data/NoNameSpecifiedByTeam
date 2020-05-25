using System;
using UnityEngine;
using System.Collections.Generic;
public class MotherShipCollisionHandler : MonoBehaviour, ISubject
{


    [Header(" --- Gameplay ---")]
    [Tooltip("How much the player gets for one piece of cargo")]
    [SerializeField] private int m_scorePerCargo = 10;

    private FSM m_Fsm;
    private BuoyFillUp m_FillUp = null;

    private List<IObserver> m_Observers = null;

    public int ScoreGain
    {
        get;
        private set;
    }
    public int LeftoverCargo
    {
        get;
        private set;
    }
    void Start()
    {
        FindTaggedObjects();
        m_FillUp = GetComponent<BuoyFillUp>();
    }

    //trys to find the object by its tag, please do not reuse the Tag, tag should be unique for this
    private void FindTaggedObjects()
    {
        m_Observers = new List<IObserver>();
        m_Observers.Add(GameObject.FindWithTag("ScoreUI").GetComponent<ScoreUI>());
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

            LeftoverCargo = m_FillUp.DropOff(cargoAmount);

            if (!m_Observers.Contains(cargo))
            {
                m_Observers.Add(cargo);
            }


            ScoreGain = ((cargoAmount - LeftoverCargo) * m_scorePerCargo);
            Notify();

            if (m_Fsm.GetCurrentState() is TutorialState currentState)
            {
                currentState.FinishTutorial();
            }
        }
    }


    public void Notify()
    {
        foreach (IObserver observer in m_Observers)
        {
            observer.GetUpdate(this);
        }
    }

    public void Attach(IObserver observer)
    {
        m_Observers.Add(observer);
    }
}
