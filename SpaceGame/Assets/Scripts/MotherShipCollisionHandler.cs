using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShipCollisionHandler : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        //check if the Trigger Participant is the Player and if he has a PlayerCargo Component 
        if (other.CompareTag("Player") && other.transform.parent.GetComponent<PlayerCargo>() is PlayerCargo cargo)
        {
            //clear all cargo
            cargo.ClearCargo();
        }
    }
}
