using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class IngameView : AbstractView
{

    [SerializeField] private TimeOutTimer m_TimeOut = null;
    public TimeOutTimer GetTimeOutTimer() => m_TimeOut;

}
