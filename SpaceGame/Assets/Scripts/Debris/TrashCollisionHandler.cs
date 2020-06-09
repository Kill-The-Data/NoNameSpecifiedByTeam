using System;
using SubjectFilters;
using UnityEngine;

[RequireComponent(typeof(TrashMovementController))]
public class TrashCollisionHandler : MonoBehaviour, ISubject
{
    [SerializeField] private bool m_IsTutorialTrash = false;
    private bool m_destroyed = false;

    private bool m_hasDealtDamage = false;

    private TrashMovementController m_controller;
    public PlayerTakeDamageFilter playerTakeDamageFilter { get; private set; }
    public PlayerPickUpTrashFilter playerPickUpTrashFilter { get; private set; }

    public void Awake()
    {
        playerTakeDamageFilter = new PlayerTakeDamageFilter();
        Attach(playerTakeDamageFilter);

        playerPickUpTrashFilter = new PlayerPickUpTrashFilter();
        Attach(playerPickUpTrashFilter);

        m_controller = GetComponent<TrashMovementController>();
        if (m_controller == null) m_controller = gameObject.AddComponent<TrashMovementController>();
    }

    public void OnTriggerEnter(Collider other)
    {
        HandlePlayerEnter(other);
        HandleTrashEnter(other);
    }

    public void OnTriggerExit(Collider other)
    {
        HandlePlayerExit(other);
        HandleTrashExit();
    }

    //Player Interaction
    private void HandlePlayerEnter(Collider other)
    {
        //check if other is connected to the player and get all the required components
        if ((other.CompareTag("Player-Collector") || other.CompareTag("Player"))
            && other.transform.parent.GetComponentSafe(out PlayerCargo playerCargo)
            && other.transform.parent.GetComponentSafe(out PlayerHealth playerHealth)
            && other.transform.parent.GetComponentSafe(out PlayerController playerController)
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
                DealDamage(playerHealth, playerController);
            }
        }
    }
    private void PickUpTrash(PlayerCargo playerCargo)
    {
        //remove the trash
        MaximumDebrisCount.RemoveDebris();

        //add some to the cargo
        playerCargo.AddCargo(this.gameObject);

        Notify(NotifyEvent.OnPlayerPickupTrash);

        //TODO(kukash): I don't quite know how, but I am sure this can be improved
        //TODO(cont.):  probably some ObserverPattern on the PlayerCargo with Notify()
        //TODO(cont.):  on AddCargo() (and probably ClearCargo() as well)
        if (m_IsTutorialTrash)
        {
            GetComponent<nextTutorialState>()?.NextState();
        }
        //only destroy if not tutorail trash
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void DealDamage(PlayerHealth playerHealth, PlayerController playerController)
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
            Notify(NotifyEvent.OnPlayerTakeDamage);
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


    //Trash Interaction
    private void HandleTrashEnter(Collider other)
    {
        if (other.CompareTag("Debris")
            && other.GetComponentSafe(out TrashMovementController trashController)
            && other.GetComponentSafe(out TrashCollisionHandler otherHandler)
            && other.GetComponentSafe(out BreakApartHandler breakApartHandler)
            && otherHandler.enabled)
        {
            if (!otherHandler.HasTheSolution())
            {
                IHaveTheSolution();
                Vector3 speed = m_controller.Speed;

                m_controller.Speed = trashController.Speed;
                trashController.Speed = speed;

                breakApartHandler.MaybeBreak();
                if (this.GetComponentSafe(out BreakApartHandler selfBreakApartHandler))
                {
                    selfBreakApartHandler.MaybeBreak();
                }
            }
        }
    }

    private void HandleTrashExit()
    {
        m_solution = false;
    }

    public enum NotifyEvent { OnPlayerTakeDamage, OnPlayerPickupTrash }
    public NotifyEvent State { get; private set; }
    private Action<ISubject> m_listeners = delegate { };
    public void Notify()
    {
        m_listeners(this);
    }
    public void Notify(NotifyEvent evnt)
    {
        State = evnt;
        Notify();
    }
    public void Attach(IObserver observer)
    {
        m_listeners += observer.GetUpdate;
    }
}


