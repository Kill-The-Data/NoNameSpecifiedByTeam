﻿using UnityEngine;
using UnityEngine.Serialization;

public class OstPlayer : MonoBehaviour
{
    [Range(0,1),Tooltip("The Volume of the Ost")]
    public float Volume = 1;

    [FormerlySerializedAs("m_ostTrack")] 
    public string OstTrack = "ost";
    private AudioSource m_source;
    
    public void Start()
    {
        m_source = gameObject.AddComponent<AudioSource>();
        m_source.clip = SoundManager.Instance.GetSound(OstTrack);
        m_source.loop = true;
        m_source.Play();
       
    }

    public void Update()
    {
        m_source.volume = Volume / 25.0f;
    }

    public void AdjustVolume(System.Single v)
    {
        Volume = v;
    }

    public void StartSong()
    {
        m_source.Play();
    }

    public void StopSong()
    {
        m_source.Stop();
    }
}
