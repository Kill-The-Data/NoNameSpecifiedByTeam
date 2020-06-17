using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(BuoyFillUp))]
[RequireComponent(typeof(AudioSource))]
public class MotherShipCollisionHandler : MonoBehaviour, ISubject
{


    [Header(" --- Gameplay ---")]
    [Tooltip("How much the player gets for one piece of cargo")]
    [SerializeField] private int m_scorePerCargo = 10;


    [Tooltip("The Sound to play on player collision")]
    [SerializeField] private string m_collisionSound = "collision-buoy";

    [FormerlySerializedAs("m_playerSound")]
    [Tooltip("The Sound to play on player dropoff")]
    [SerializeField] private string m_dropOffSound;
    [SerializeField] private bool m_UseConfigForScoreGain = true;

    private FSM m_Fsm;
    public BuoyFillUp m_FillUp = null;

    public event Action<ISubject> trashCollected;
    public event Action<ISubject> collision;


    private AudioSource m_collisionSource;
    private AudioSource m_dropOffSource;

    private float m_returnVolume;

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
        m_dropOffSource = gameObject.GetComponent<AudioSource>();
        m_collisionSource = gameObject.AddComponent<AudioSource>();
        m_collisionSource.volume = 0;

        if (m_UseConfigForScoreGain)
            WebConfigHandler.OnFinishDownload(o =>
            {
                o.ExtractInt("Normal_Trash_Score", value => m_scorePerCargo = value);
            });
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
            m_dropOffSource.clip = manager.GetSound(m_dropOffSound);
            m_collisionSource.clip = manager.GetSound(m_collisionSound);
            m_returnVolume = manager.GetFxVolume();
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
        m_dropOffSource.volume = 0;
        m_collisionSource.volume = 0;
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

            m_collisionSource.volume = m_returnVolume;
            m_collisionSource.Play();

            StartCoroutine(MuteAudio());

            if (m_FillUp.GetState() == BuoyFillUp.BuoyCargoState.FULL) return;

            //get cargo
            int cargoAmount = cargo.SpaceOccupied;

            if (cargoAmount == 0) return;
            //drop off
            LeftoverCargo = m_FillUp.DropOff(cargoAmount);


            //play audio
            m_dropOffSource.volume = m_returnVolume;
            m_dropOffSource.Play();


            if (!m_wasCargoAdded)
            {
                m_wasCargoAdded = true;
                trashCollected += cargo.GetUpdate;
            }
            ScoreGain = ((cargoAmount - LeftoverCargo) * m_scorePerCargo);
            trashCollected?.Invoke(this);
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
