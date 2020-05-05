using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShipCollisionHandler : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.GetComponent<PlayerCargo>())
        {
            PlayerCargo playerCargo = other.transform.parent.GetComponent<PlayerCargo>();
            playerCargo.ClearCargo();
        }
    }
}
