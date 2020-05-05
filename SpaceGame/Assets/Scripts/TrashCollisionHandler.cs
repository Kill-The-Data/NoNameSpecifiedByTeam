using UnityEngine;

public class TrashCollisionHandler : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.transform.parent.GetComponent<PlayerCargo>() is PlayerCargo playerCargo)
        {
            if (!playerCargo.SpaceIsFull()) 
            {

               Destroy(this.transform.gameObject);
                playerCargo.AddCargo();
            }
        }
    }
}
