using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class SoundListener : AUnityObserver
{
    [SerializeField] private string m_sound;

    private AudioClip m_clip;
    private AudioSource m_source;
    
    public void Start()
    {
        m_clip = SoundManager.Instance.GetSound(m_sound);
        m_source = gameObject.AddComponent<AudioSource>();
       
        
        m_source.playOnAwake = false;
        m_source.clip = m_clip;
    }

    protected override void AGetUpdate(ISubject subject)
    {   
        m_source.pitch = UnityEngine.Random.Range(0.1F, 1.1F);
        m_source.Play();
    }
}
