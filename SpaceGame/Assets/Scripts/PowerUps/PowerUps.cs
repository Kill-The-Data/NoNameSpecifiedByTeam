using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class PowerUp : MonoBehaviour
{
    public abstract void ExecutePowerUp(PlayerScriptContainer pHandler);
}

public abstract class SoundPowerUp : PowerUp
{
    [Tooltip("The sound to play on pickup")]
    [SerializeField] protected string m_pickupSound;

    private static GameObject m_audioPlayerChild;
    
    private static  AudioSource m_source;

    public void Start()
    {
        if (m_audioPlayerChild != null)
            m_source = m_audioPlayerChild.GetComponent<AudioSource>();
    }
    
    public override void ExecutePowerUp(PlayerScriptContainer pHandler)
    {
        if (m_audioPlayerChild == null)
        {
            m_audioPlayerChild = new GameObject();
            m_audioPlayerChild.transform.parent = pHandler.PlayerObject.transform;
            m_source = m_audioPlayerChild.AddComponent<AudioSource>();
            m_source.playOnAwake = false;
        }
        
        m_source.clip = SoundManager.Instance.GetSound(m_pickupSound);
        m_source.Play();
    }
}