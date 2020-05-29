using System;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(BuoyFillUp))]
public class MotherShipCollisionHandler : MonoBehaviour, ISubject
{


    [Header(" --- Gameplay ---")]
    [Tooltip("How much the player gets for one piece of cargo")]
    [SerializeField] private int m_scorePerCargo = 10;

    [Tooltip("The Sound to play on player collision")]
    [SerializeField] private string m_playerSound;


    private FSM m_Fsm;
    private BuoyFillUp m_FillUp = null;

    private List<IObserver> m_Observers = null;

    private AudioSource m_source;

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
        m_Observers = new List<IObserver>();
        FindTaggedObjects();
        m_FillUp = GetComponent<BuoyFillUp>();
        m_source = gameObject.AddComponent<AudioSource>();
        m_source.clip = SoundManager.Instance.GetSound(m_playerSound);
    }

    //trys to find the object by its tag, please do not reuse the Tag, tag should be unique for this
    private void FindTaggedObjects()
    {
        m_Observers = new List<IObserver>();
        GameObject obj = GameObject.FindWithTag("ScoreUI");
        m_Observers.Add(obj.GetComponent<ScoreUI>());
        if (!m_Fsm)
            m_Fsm = GameObject.FindWithTag("FSM").GetComponent<FSM>();

    }
    public void OnTriggerEnter(Collider other)
    {
        //check if the Trigger Participant is the Player and if he has a PlayerCargo Component 
        if (other.CompareTag("Player")
            && other.transform.parent.GetComponentSafe(out PlayerCargo cargo))
        {
            int cargoAmount = cargo.SpaceOccupied;

            LeftoverCargo = m_FillUp.DropOff(cargoAmount);
            if (LeftoverCargo != 0)
                m_source.Play();

            if (!m_Observers.Contains(cargo))
                m_Observers.Add(cargo);


            ScoreGain = ((cargoAmount - LeftoverCargo) * m_scorePerCargo);
            Notify();

            if (m_Fsm.GetCurrentState() is TutorialState currentState)
                currentState.FinishTutorial();
            
        }
    }


    public void Notify()
    {

        IObserver expiredObserver = null;
        bool foundNullObserver =false;
        foreach (IObserver observer in m_Observers)
        {
            if (observer != null)
            {
                observer.GetUpdate(this);
            }
            //remove null observers
            else
            {
                expiredObserver = observer;
                foundNullObserver=true;
            }
        }
       if(foundNullObserver)
            m_Observers.Remove(expiredObserver);
    }

    public void Attach(IObserver observer)
    {
        m_Observers.Add(observer);
    }
}
