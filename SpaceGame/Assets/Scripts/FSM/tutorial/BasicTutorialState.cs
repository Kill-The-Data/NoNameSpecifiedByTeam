using UnityEngine;

public class BasicTutorialState : AbstractTutorialState
{
    [SerializeField] private string m_StateName;
    //public get & private set for state name
    public string StateName
    {
        get => m_StateName;
        private set => m_StateName = value;
    }
    //next state to be loaded
    [SerializeField] private BasicTutorialState m_NextState = null;
    //leaves state and sets next state
    //This should be called after trigger / button / event gets activated to load next state of tutorial
    public void NextState()
    {
        ExitState();
        if (m_NextState)
        {
            m_tFSM.ChangeState(m_NextState.StateName);
        }
    }
    public override void EnterState()
    {
        view?.EnableView();
    }

}
