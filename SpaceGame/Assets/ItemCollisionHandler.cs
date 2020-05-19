using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ItemCollisionHandler : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Player")) 
        {
        //TO DO: DO STUFF
        Destroy(transform.gameObject);
        }
    }
}
