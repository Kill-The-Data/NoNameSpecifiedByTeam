using System;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(AudioSource),typeof(ObstacleMover))]
public class ObstacleCollisionHandler : AbstractCollider, ISubject
{

    [Tooltip("The Sound to play on Collision, checkout the GlobalSetup->SoundManager")]
    [LabelOverride("Sound on Collision")]
    [SerializeField] private string m_soundId = "collision";
    [SerializeField] private GameObject m_CollisionParticlePrefab = null;

    [SerializeField] private bool m_doNoEvilCauseNoHarm = false;
    
    private int m_damage = 10;

    private AudioSource m_audioSource;
    private ObstacleMover m_mover;

    public void Awake()
    {
        m_audioSource = gameObject.GetComponent<AudioSource>();
        m_mover = gameObject.GetComponent<ObstacleMover>();
        
        SoundManager.ExecuteOnAwake(manager =>
        {
            m_audioSource.clip = manager.GetSound(m_soundId);
            m_audioSource.volume = manager.GetFxVolume();
        });

        WebConfigHandler.OnFinishDownload(o =>
        {
            o.ExtractInt("obstacle_damage", v => m_damage = v);
        });

    }


    private Action<ISubject> m_listeners = delegate { };
    private bool m_hasDealtDamage;
   
    private float m_time = 0;

    protected override void HandlePlayerStay(Collider other)
    {
        if ((other.CompareTag("Player") || other.CompareTag("Player-Collector"))
            && other.transform.parent.GetComponentSafe(out PlayerController playerController))
        {
            playerController.ResolveCollision(other.transform.position-transform.position);
            //playerController.ReplayCollision(m_time);
            m_time += Time.deltaTime;
            if (m_time > 0.5F)
            {
                //make sure the player does not get stuck
                playerController.Enable();
            }
        }
    }

    protected override void HandleObstacleEnter(Collider other)
    {
        return;
        if (other.CompareTag("Obstacles") &&
            other.GetComponentSafe(out ObstacleMover mover))
        {
            var temp = m_mover.Speed;
            m_mover.Speed = mover.Speed;
            mover.Speed = temp;
        }
    }


    protected override void HandlePlayerEnter(Collider other)
    {
        if ((other.CompareTag("Player") || other.CompareTag("Player-Collector"))
            && other.transform.parent.GetComponentSafe(out PlayerHealth playerHealth)
            && other.transform.parent.GetComponentSafe(out PlayerController playerController)
        )
        {
            PlaySound();
            DealDamage(playerHealth, playerController);

            if (m_CollisionParticlePrefab)
            {
                Vector3 POI = other.ClosestPoint(transform.position);
                Instantiate(m_CollisionParticlePrefab, POI, Quaternion.identity);
            }
        }
    }

    private void PlaySound()
    {
        if (m_audioSource)
        {
            m_audioSource.Play();
        }
    }

    private void DealDamage(PlayerHealth health, PlayerController controller)
    {
        if (health == null || controller == null) return;
        if (!m_hasDealtDamage)
        {
            if(!m_doNoEvilCauseNoHarm){
                health.TakeDamage(m_damage);
            }
            //make the factor so big you actually bounce back
            var velocity = controller.GetVelocity();

            var direction = velocity.normalized;
            var magnitude = Mathf.Min(velocity.magnitude, 1);

            m_mover.Speed += direction * magnitude;
            
            controller.Collide(1.4f);
            controller.Disable();

            m_hasDealtDamage = true;
            Notify();
        }
    }

    protected override void HandlePlayerLeave(Collider other)
    {
        if ((other.CompareTag("Player") || other.CompareTag("Player-Collector"))
        && other.transform.parent.GetComponentSafe(out PlayerController playerController))
        {
            m_hasDealtDamage = false;
            playerController.Enable();
            m_time = 0;
        }
    }


    public void Attach(IObserver observer)
    {
        m_listeners += observer.GetUpdate;
    }

    public void Notify()
    {
        m_listeners(this);
    }

}
