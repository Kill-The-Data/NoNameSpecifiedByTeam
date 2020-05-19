using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraTimePowerUp : PowerUp
{
    [SerializeField] private float m_timeGain = 10.0f;
    public override void ExecutePowerUp(PlayerScriptContainer pHandler)
    {
        pHandler.GetTimer?.IncreaseTimeLeft(m_timeGain);
    }
}
