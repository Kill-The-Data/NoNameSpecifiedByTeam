using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class FSM : MonoBehaviour
{

    private Dictionary<Type, State> m_stateMap = new Dictionary<Type, State>();
    private State m_CurrentState = null;
    [SerializeField] private State m_StartState = null;

    //init fsm on awake
    private void Awake()
    {
        Debug.Log("init FSM");
        Init();
    }
    //set start state 
    void Start()
    {
        ChangeState(m_StartState.GetType());
    }

    //get all states in children
    private void Init()
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
    private void ChangeState(Type type)
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


}
