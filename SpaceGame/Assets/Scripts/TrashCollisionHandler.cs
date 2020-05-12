using UnityEngine;

public class TrashCollisionHandler : MonoBehaviour
{

    [SerializeField] private bool m_IsTutorialTrash = false;
    private bool m_destroyed = false;

    private bool m_hasDealtDamage = false;

    private TrashMovementController m_controller;

    public void Start()
    {
        m_controller = GetComponent<TrashMovementController>();
        if (m_controller == null) m_controller = gameObject.AddComponent<TrashMovementController>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player-Collector") || other.CompareTag("Player"))
            && other.transform.parent.GetComponent<PlayerCargo>() is PlayerCargo playerCargo
            && other.transform.parent.GetComponent<PlayerHealth>() is PlayerHealth playerHealth
            && other.transform.parent.GetComponent<PlayerController>() is PlayerController playerController
            )
        {
            if (!playerCargo.SpaceIsFull() && other.CompareTag("Player-Collector"))
            {
                Destroy(this.gameObject);
                playerCargo.AddCargo();

                if (m_IsTutorialTrash)
                {
                    GetComponent<nextTutorialState>()?.NextState();
                }

            }
            else
            {
                if (!m_hasDealtDamage)
                {
                    playerHealth.TakeDamage();
                    m_controller.Speed += playerController.Collide();
                    m_hasDealtDamage = true;

                }

            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player-Collector"))
        {
            m_hasDealtDamage = false;
        }
    }
}
