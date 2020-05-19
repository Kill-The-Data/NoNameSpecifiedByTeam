using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ItemCollisionHandler : MonoBehaviour
{
    private PowerUp m_powerUp;
    private void Awake()
    {
        m_powerUp = GetComponent<PowerUp>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Player"))
        {
            PlayerScriptContainer pHandler = other.GetComponent<PlayerScriptContainer>();
            if (pHandler)
                m_powerUp?.ExecutePowerUp(pHandler);

            //TO DO: DO STUFF
            Destroy(transform.gameObject);
        }
    }
}

