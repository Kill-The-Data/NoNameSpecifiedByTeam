using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShipCollisionHandler : MonoBehaviour
{
    [SerializeField] ScoreUI m_scoreUI;
    public void OnTriggerEnter(Collider other)
    {
        //check if the Trigger Participant is the Player and if he has a PlayerCargo Component 
        if (other.CompareTag("Player") && other.transform.parent.GetComponent<PlayerCargo>() is PlayerCargo cargo)
        {
            int cargoAmount = cargo.m_spaceOccupied;
            m_scoreUI.AddScore(cargoAmount * 10);
            //clear all cargo
            cargo.ClearCargo();
            
        }
    }
}
