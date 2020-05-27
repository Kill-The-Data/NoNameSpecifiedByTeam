using UnityEngine;

public class nextTutorialState : MonoBehaviour
{
    [SerializeField] private BasicTutorialState m_currentState;
    public void NextState()
    {
        Destroy(this.transform.parent.gameObject);
        m_currentState.NextState();
    }
}