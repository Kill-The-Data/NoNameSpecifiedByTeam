using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraTimePowerUp : SoundPowerUp
{
    [SerializeField] private float m_timeGain = 10.0f;
    public override void ExecutePowerUp(PlayerScriptContainer pHandler)
    {
        base.ExecutePowerUp(pHandler);
        pHandler.GetTimer?.IncreaseTimeLeft(m_timeGain);
    }
}
