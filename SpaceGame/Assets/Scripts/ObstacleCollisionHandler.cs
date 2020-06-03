using System;
using SubjectFilters;
using UnityEngine;

public class ObstacleCollisionHandler : MonoBehaviour , ISubject
{
  
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
            DealDamage(playerHealth,playerController);
        }
    }

    private void DealDamage(PlayerHealth health, PlayerController controller)
    {
        if (health == null || controller == null) return;

        if (!m_hasDealtDamage)
        {
            health.TakeDamage();
            
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
