using UnityEngine;

public class TrashCollisionHandler : MonoBehaviour
{

    [SerializeField] private bool m_IsTutorialTrash = false;
    private bool m_destroyed = false;

    private bool m_hasDealtDamage = false;

    private TrashMovementController m_controller;

    public void Awake()
    {
        m_controller = GetComponent<TrashMovementController>();
        if (m_controller == null) m_controller = gameObject.AddComponent<TrashMovementController>();
    }

    public void OnTriggerEnter(Collider other)
    {
       HandlePlayer(other);
       HandleOtherTrash(other);
    }

    public void OnTriggerExit(Collider other)
    {
        HandlePlayerExit(other);
        ResetSolution();
    }

    //Player Interaction
    private void HandlePlayer(Collider other)
    {
        //check if other is connected to the player and get all the required components
        if ((other.CompareTag("Player-Collector") || other.CompareTag("Player"))
            && other.transform.parent.GetComponentSafe<PlayerCargo>(out var playerCargo) 
            && other.transform.parent.GetComponentSafe<PlayerHealth>(out var playerHealth) 
            && other.transform.parent.GetComponentSafe<PlayerController>(out var playerController)
        )
        {
            //check if the player has space in his cargo and if he collided with the cargo collider
            if (!playerCargo.SpaceIsFull() && other.CompareTag("Player-Collector"))
            {
                //pickup the trash 
               PickUpTrash(playerCargo);
            }
            else
            {
                //otherwise take damage
                DealDamage(playerHealth,playerController);
            }
        }
    }
    private void PickUpTrash(PlayerCargo playerCargo)
    {
        //remove the trash
        MaximumDebrisCount.RemoveDebris();
        Destroy(this.gameObject);
        
        //add some to the cargo
        playerCargo.AddCargo();
        
        //TODO(kukash): I don't quite know how, but I am sure this can be improved
        //TODO(cont.):  probably some ObserverPattern on the PlayerCargo with Notify()
        //TODO(cont.):  on AddCargo() (and probably ClearCargo() as well)
        if (m_IsTutorialTrash)
        {
            GetComponent<nextTutorialState>()?.NextState();
        }
    }
    private void DealDamage(PlayerHealth playerHealth,PlayerController playerController)
    {
        if (playerHealth == null || playerController == null) return;
        //check if this specific barrel has already dealt damage to the player
        if (!m_hasDealtDamage)
        {
            //deal damage to the player
            playerHealth.TakeDamage();
            
            //do some speed-transfer with the barrel.
            m_controller.Speed += playerController.Collide();
            m_hasDealtDamage = true;

        }
    }

    private void HandlePlayerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player-Collector"))
        {
            m_hasDealtDamage = false;
        }
    }

    private bool m_solution = false;
    private void IHaveTheSolution()
    {
        m_solution = true;
    }
    private bool HasTheSolution()
    {
        return m_solution;
    }
    private void ResetSolution()
    {
        m_solution = false;
    }
    
    //Trash Interaction
    private void HandleOtherTrash(Collider other)
    {
        if (other.CompareTag("Debris")
            && other.GetComponentSafe<TrashMovementController>(out var trashController)
            && other.GetComponentSafe<TrashCollisionHandler>(out var otherHandler)
            && other.GetComponentSafe<BreakApartHandler>(out var breakApartHandler)
            && otherHandler.enabled)
        {
            if(!otherHandler.HasTheSolution())
            {
                IHaveTheSolution();
                Vector3 speed = m_controller.Speed;
                
                m_controller.Speed = trashController.Speed;
                trashController.Speed = speed;

                breakApartHandler.MaybeBreak();
                if (this.GetComponentSafe<BreakApartHandler>(out var selfBreakApartHandler))
                {
                    selfBreakApartHandler.MaybeBreak();
                }
            }
        }
    }
}
