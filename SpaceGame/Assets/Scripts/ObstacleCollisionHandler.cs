using System;
using SubjectFilters;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ObstacleCollisionHandler : MonoBehaviour , ISubject
{
    
    [Tooltip("The Sound to play on Collision, checkout the GlobalSetup->SoundManager")]
    [LabelOverride("Sound on Collision")]
    [SerializeField] private string m_soundId ="collision";

    private int m_damage = 10;
    
    private AudioSource m_audioSource;
    
    public void Awake()
    {
        m_audioSource = gameObject.GetComponent<AudioSource>();
        
        SoundManager.ExecuteOnAwake(manager =>
        {
            m_audioSource.clip = manager.GetSound(m_soundId);
        });
        
        WebConfigHandler.OnFinishDownload(o =>
        {
            o.ExtractInt("obstacle_damage", v => m_damage = v);
        });
        
    }


    private Action<ISubject> m_listeners = delegate {  };
    private bool m_hasDealtDamage;
    public void OnTriggerEnter(Collider other)
    {
        HandlePlayerEnter(other);
    }
    public void OnTriggerExit(Collider other)
    {
        HandlePlayerExit(other);
    }


    private void HandlePlayerEnter(Collider other)
    {
        if ((other.CompareTag("Player") || other.CompareTag("Player-Collector"))
            && other.transform.parent.GetComponentSafe(out PlayerHealth playerHealth)
            && other.transform.parent.GetComponentSafe(out PlayerController playerController)
        )
        {
            PlaySound();
            DealDamage(playerHealth,playerController);
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
            health.TakeDamage(m_damage);
            
            //make the factor so big you actually bounce back
            controller.Collide(1.4f);
            
            m_hasDealtDamage = true;
            Notify();
        }
    }

    private void HandlePlayerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player-Collector"))
        {
            m_hasDealtDamage = false;
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
