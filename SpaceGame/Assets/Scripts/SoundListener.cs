using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundListener : MonoBehaviour , IObserver
{
    [SerializeField] private string m_sound;

    private AudioClip m_clip;
    private AudioSource m_source;
    
    public void Start()
    {
        m_clip = SoundManager.Instance.GetSound(m_sound);
        m_source = gameObject.AddComponent<AudioSource>();
        m_source.clip = m_clip;
    }
    
    public void GetUpdate(ISubject subject)
    {
        m_source.Play();
    }
}
