using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(BuoyFillUp))]
[RequireComponent(typeof(AudioSource))]
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

    private static AudioSource m_cursedAudioSource;
    
    //HACK:(Algo-ryth-mix): by all means try to change it
    //numberOfHoursWastedWithThis = 4
    private static int buoyNumber = 0;
    
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
        m_FillUp = GetComponent<BuoyFillUp>();
        m_source = gameObject.GetComponent<AudioSource>();
        /*
        //for whatever unholy reason the first audio-source does not want to play,
        //but we can cheat by making one of them play on awake
        if (buoyNumber != 0)
        {
            m_source.playOnAwake = false;
        }
        else
        {
            m_cursedAudioSource = m_source;
        }
        
        ++buoyNumber;
        */
        //also by some arcane dark ass magic,
        //this Start() is actually somehow called before Awake()
        //I do not know why, I do not question why, I just mentally noted it
        // (and literally obviously, as you are reading it rn)
        
        
        //also this garbage ( yes I know this is my creation )
        SoundManager.ExecuteOnAwake(manager =>
        {
            m_source.clip = manager.GetSound(m_playerSound);
        });
        
        m_Observers = new List<IObserver>();
        FindTaggedObjects();
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

    private IEnumerator MuteAudio()
    {
        //as we all know exorcism takes some while
        yield return new WaitForSeconds(3);

        //and you are healed
        m_source.volume = 0;
    }
    
    public void OnTriggerEnter(Collider other)
    {
        //check if the Trigger Participant is the Player and if he has a PlayerCargo Component 
        if (other.CompareTag("Player")
            && other.transform.parent.GetComponentSafe(out PlayerCargo cargo))
        {
            if(m_FillUp.Full()) return;

            //get cargo
            int cargoAmount = cargo.SpaceOccupied;

            if (cargoAmount == 0) return;
            //drop off
            LeftoverCargo = m_FillUp.DropOff(cargoAmount);

            
            //play audio
            m_source.volume = 0.5f;
            m_source.Play();
            StartCoroutine(MuteAudio());
            
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
