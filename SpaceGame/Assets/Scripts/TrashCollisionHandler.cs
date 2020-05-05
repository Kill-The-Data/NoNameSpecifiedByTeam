using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCollisionHandler : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.GetComponent<PlayerCargo>())
        {
            PlayerCargo playerCargo = other.transform.parent.GetComponent<PlayerCargo>();
            if (!playerCargo.SpaceIsFull()) 
            {
               Destroy(this.transform.gameObject);
                playerCargo.AddCargo();
            }
        }
    }
}
