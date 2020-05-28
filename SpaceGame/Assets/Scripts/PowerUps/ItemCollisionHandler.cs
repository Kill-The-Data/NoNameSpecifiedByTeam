using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ItemCollisionHandler : MonoBehaviour
{

    private PowerUp pUp = null;

    private void Awake()
    {
        pUp = GetComponent<PowerUp>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerScriptContainer pHandler = other.GetComponent<PlayerScriptContainer>();
            if(pHandler)
                pUp?.ExecutePowerUp(pHandler);
            
            //TO DO: DO STUFF
            Destroy(transform.gameObject);
        }
    }
}
