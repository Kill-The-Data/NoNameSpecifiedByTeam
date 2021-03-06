﻿using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class SoundListener : AUnityObserver
{
    [SerializeField] private string m_sound;
    [SerializeField] private bool m_randomPitch = false;
    [Range(0,1)]
    [SerializeField] private float m_volume = 1;
    
    private AudioClip m_clip;
    private AudioSource m_source;
    
    public void Awake()
    {
        SoundManager.ExecuteOnAwake(manager => {
            m_clip = manager.GetSound(m_sound);
            m_source = gameObject.AddComponent<AudioSource>();
            m_source.volume = manager.GetFxVolume();
            m_source.playOnAwake = false;
            m_source.clip = m_clip;
        });
    }

    public void Update()
    {
        m_source.volume = m_volume;
    }

    protected override void AGetUpdate(ISubject subject)
    {   
        if(m_randomPitch)
            m_source.pitch = UnityEngine.Random.Range(0.7F, 1.2F);
        m_source.Play();
    }
}
