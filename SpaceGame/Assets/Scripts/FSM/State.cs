using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected FSM fsm { get; private set; }

    public virtual void Initialize(FSM newFsm)
    {
        fsm = newFsm;
        gameObject.SetActive(false);
    }
    public virtual void EnterState()
    {
        Debug.Log("Entering state : " + this);
        gameObject.SetActive(true);
    }

    public virtual void ExitState() 
    {
        Debug.Log("leaving state : " + this);
        gameObject.SetActive(false);
    }
}
