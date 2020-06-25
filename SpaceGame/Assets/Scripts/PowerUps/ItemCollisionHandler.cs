using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using EventHandler = SpaceGame.EventHandler;

public class ItemCollisionHandler : MonoBehaviour
{

    [SerializeField] private bool DontDestroy = false;
    
    private PowerUp pUp = null;

    private void Awake()
    {
        pUp = GetComponent<PowerUp>();

        EventHandler.Instance.TutorialStart += Reset;
    }

    private void OnDestroy()
    {
        EventHandler.Instance.TutorialStart -= Reset;
    }
    private void Reset()
    {
        if (DontDestroy)
        {
            gameObject.SetActive(true);
            GetComponent<Collider>().enabled = true;
        }
    }
    
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerScriptContainer pHandler = other.GetComponent<PlayerScriptContainer>();
            if(pHandler)
                pUp?.ExecutePowerUp(pHandler);


            if (DontDestroy)
            {
                gameObject.SetActive(false);
                GetComponent<Collider>().enabled = false;
            }
            else
            {
                Destroy(transform.gameObject);
            }
        }
    }
}
