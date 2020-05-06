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

        //deduplication fix for voronoi noise
        if (other.CompareTag("Debris") && other.GetComponent<TrashCollisionHandler>() is TrashCollisionHandler handler)
        {
            //make sure we don't collide twice
            if (handler.m_destroyed) return;
            m_destroyed = true;
            
            Destroy(this.gameObject);
        }
    }
}
