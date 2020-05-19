using UnityEngine;

public abstract class StateWithView<T> : State where T : AbstractView
{
    [SerializeField] private T m_View = null;
    protected T view => m_View;

    public override void EnterState()
    {
        base.EnterState();
        view?.EnableView();
    }
    public override void ExitState()
    {
        base.ExitState();
        view?.DisableView();
    }

    public override void Initialize(FSM newFSM)
    {
        base.Initialize(newFSM);
        view?.DisableView();
    }
}
