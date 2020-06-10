using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(GoalReachedHandler))]
public class FinishCollisionHandler : MonoBehaviour
{
    private GoalReachedHandler m_goalReachedHandler = null;

    private void Start()
    {
        m_goalReachedHandler = GetComponent<GoalReachedHandler>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FinishGame();
        }
    }

    private void FinishGame()
    {
        m_goalReachedHandler.OnGoalReached();
    }
}
