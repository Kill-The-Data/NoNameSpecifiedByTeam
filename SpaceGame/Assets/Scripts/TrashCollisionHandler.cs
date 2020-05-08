using UnityEngine;

public class TrashCollisionHandler : MonoBehaviour
{
    private bool m_destroyed = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.transform.parent.GetComponent<PlayerCargo>() is PlayerCargo playerCargo)
        {
            if (!playerCargo.SpaceIsFull()) 
            {
                Destroy(this.gameObject);
               playerCargo.AddCargo();
            }
        }
    }
}
