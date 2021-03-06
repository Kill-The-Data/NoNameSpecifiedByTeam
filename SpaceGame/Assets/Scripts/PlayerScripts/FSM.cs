﻿using System.Collections.Generic;
using UnityEngine;
using System;
public class FSM : MonoBehaviour
{

    private Dictionary<Type, State> m_stateMap = new Dictionary<Type, State>();
    protected State m_CurrentState = null;
    [SerializeField] protected State m_startState = null;

    //init fsm on awake
    private void Awake()
    {
        Init();
    }
    //set start state 
    void Start()
    {
        ChangeState(m_startState.GetType());
    }

    //get all states in children
    protected virtual void Init()
    {
        State[] states = GetComponentsInChildren<State>(true);

        foreach (State currentState in states)
        {
            m_stateMap.Add(currentState.GetType(), currentState);
            currentState.Initialize(this);
        }
    }

    public void ChangeState<T>() where T : State
    {
        //delegate to change state
        ChangeState(typeof(T));
    }

    //changes state
    protected void ChangeState(Type type)
    {
        //return if state is active state
        if (m_CurrentState != null && m_CurrentState.GetType() == type) return;
        //exit state if not null
        if (m_CurrentState != null)
        {
            m_CurrentState.ExitState();
            m_CurrentState = null;
        }

        //enter next state if not null and contained in map
        if (type != null && m_stateMap.ContainsKey(type))
        {
            m_CurrentState = m_stateMap[type];
            m_CurrentState.EnterState();
        }
    }
    public State GetCurrentState()
    {
        return m_CurrentState;
    }
}
