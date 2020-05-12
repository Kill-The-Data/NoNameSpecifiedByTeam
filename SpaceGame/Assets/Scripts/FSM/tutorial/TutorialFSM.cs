using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFSM : FSM
{
    [SerializeField] private State m_ParentState = null;
    // [SerializeField] private BasicTutorialState m_StartState = null;
    private Dictionary<string, State> m_stateMap = new Dictionary<string, State>();
    //loads start state
    public void InitTutorial()
    {
        if (m_startState is BasicTutorialState state)
            ChangeState(state.StateName);
    }
    //overwrites FSM init, now uses specified name instead of type
    //this enables the statemachine to use multiple states with the same type
    protected override void Init()
    {
        BasicTutorialState[] states = GetComponentsInChildren<BasicTutorialState>(true);

        foreach (BasicTutorialState currentState in states)
        {
            m_stateMap.Add(currentState.StateName, currentState);
            currentState.Initialize(this);
        }
    }
    //overwrites state changing
    //now uses state name as input to enable multiple states with same type
    public void ChangeState(string name)
    {
        //return if state is active state
        if (m_CurrentState != null && m_CurrentState is BasicTutorialState state)
            if (state.StateName == name) return;

        //exit state if not null
        if (m_CurrentState != null)
        {
            m_CurrentState.ExitState();
            m_CurrentState = null;
        }

        //enter next state if not null and contained in map
        if (name != null && m_stateMap.ContainsKey(name))
        {
            m_CurrentState = m_stateMap[name];
            m_CurrentState.EnterState();
        }
    }
    public void FinishTutorial() 
    {
        Debug.Log("leaving state");
        gameObject.SetActive(false);
        m_ParentState.ExitState();
    }

}
