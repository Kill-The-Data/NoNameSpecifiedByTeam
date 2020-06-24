using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    private AudioSource m_audioSource = null;
    [SerializeField] private string m_songOfMyPeople;
    private AbstractCollider m_collider;
    private void Awake()
    {
        
        
        SoundManager.ExecuteOnAwake(instance =>
        {
            m_audioSource = instance.FetchMeAnOutput($"source for {m_songOfMyPeople}");
            if(m_audioSource.clip == null)
                m_audioSource.clip = instance.GetSound(m_songOfMyPeople);
        });
        
        m_collider = GetComponent<AbstractCollider>();
        m_collider.OnPlayerCollide += player => m_audioSource?.Play();;
    }
}
