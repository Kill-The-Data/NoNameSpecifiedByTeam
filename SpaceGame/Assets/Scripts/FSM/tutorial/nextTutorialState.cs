using UnityEngine;

public class nextTutorialState : MonoBehaviour
{
    [SerializeField] private BasicTutorialState m_currentState;
    public void NextState()
    {
        m_currentState.NextState();
    }
}