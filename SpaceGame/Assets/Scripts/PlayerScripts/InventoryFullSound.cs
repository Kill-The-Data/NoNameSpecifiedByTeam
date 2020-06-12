using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryFullSound : MonoBehaviour
{
    private AudioSource m_source;

    [SerializeField] private string m_sound;
    [SerializeField] private PlayerCargo m_cargo;
    
    
    void Start()
    {
        m_source = new GameObject("AudioChildFullInv",typeof(AudioSource)).GetComponent<AudioSource>();
        m_source.playOnAwake = false;
        m_source.transform.parent = transform;
        SoundManager.ExecuteOnAwake(manager =>
        {
            m_source.clip = manager.GetSound(m_sound);
        });
    }


    private bool m_wasFullTheLastTimeIChecked = false;
    
    // Update is called once per frame
    void Update()
    {
        bool is_full = m_cargo.SpaceIsFull();
        
        if (!m_wasFullTheLastTimeIChecked && is_full)
        {
            m_source.Play();
        }

        m_wasFullTheLastTimeIChecked = is_full;
    }
}
