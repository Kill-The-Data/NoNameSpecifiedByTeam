using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateWithView<T> : State where T : AbstractView
{
    [SerializeField] private T m_View = null;
    protected T view { get { return m_View; } }
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

    public override void Initialize(FSM pFsm)
    {
        base.Initialize(pFsm);
        view?.DisableView();
    }
}
