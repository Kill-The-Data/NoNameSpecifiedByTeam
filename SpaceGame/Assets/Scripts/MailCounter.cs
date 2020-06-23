using System;
using UnityEngine;


// This is the fanciest fucking int 
// you have ever seen
public class MailCounter : MonoBehaviour
{
    private static MailCounter m_instance;
    private static Action<MailCounter> m_instanceCreatedActions;
    
    //it has overflow protection
    [SerializeField] private int m_maxMail;
    
    //events
    public event Action<int> OnMailReceived;
    public event Action<int> OnMailFilled;

    private int m_count = 0;

    // and custom accessors
    public int Count
    {
        get => m_count;
        private set => SetCountAndUpdateEvents(value);
    }

    public int MaxMail => m_maxMail;


    //it is fucking guarded global static
    private void Awake()
    {
        m_instance = this;
        m_instanceCreatedActions?.Invoke(this);
        m_instanceCreatedActions = null;
    }

    //and uses and action based instance subscription model
    public static void OnInstance(Action<MailCounter> o)
    {
        if (m_instance)
            o(m_instance);
        else
        {
            m_instanceCreatedActions += o;
        }
    }
    
    //look at this!!
    private void SetCountAndUpdateEvents(int value)
    {
        if (value >= m_maxMail)
        {
            m_count = m_maxMail;
            OnMailFilled?.Invoke(m_count);
        }
        else
        {
            m_count = value;
        }
        OnMailReceived?.Invoke(m_count);
    }
    
    public void AddMail() => Count++;
}
