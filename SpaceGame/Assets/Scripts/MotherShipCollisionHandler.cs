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

    public event Action<ISubject> trashCollected;
    public event Action<ISubject> collision;
    

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
        
        FindTaggedObjects();
    }

    //trys to find the object by its tag, please do not reuse the Tag, tag should be unique for this
    private void FindTaggedObjects()
    {
        m_wasCargoAdded = false;
        trashCollected = null;
        GameObject obj = GameObject.FindWithTag("ScoreUI");
        trashCollected += obj.GetComponent<ScoreUI>().GetUpdate;
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

    private bool m_wasCargoAdded = false;
    
    public void OnTriggerEnter(Collider other)
    {
        //check if the Trigger Participant is the Player and if he has a PlayerCargo Component 
        if ((other.CompareTag("Player") || other.CompareTag("Player-Collector")) 
            && other.transform.parent.GetComponentSafe(out PlayerCargo cargo)
            && other.transform.parent.GetComponentSafe(out PlayerController playerController))
        {

            playerController.Collide(1.5F);
            collision?.Invoke(this);
            
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

            if (!m_wasCargoAdded)
            {
                m_wasCargoAdded = true;
                trashCollected += cargo.GetUpdate;
            }
            ScoreGain = ((cargoAmount - LeftoverCargo) * m_scorePerCargo);
            trashCollected?.Invoke(this);

            if (m_Fsm.GetCurrentState() is TutorialState currentState)
                currentState.FinishTutorial();
            
        }
    }

    public void Notify()
    {
        throw new NotImplementedException();
    }

    [Obsolete("Please subscribe to the event directly in the future!")]
    public void Attach(IObserver observer)
    {
        trashCollected += observer.GetUpdate;
    }
}
