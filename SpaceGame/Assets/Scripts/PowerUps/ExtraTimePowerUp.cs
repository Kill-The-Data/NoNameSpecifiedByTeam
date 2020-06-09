using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ExtraTimePowerUp : SoundPowerUp
{
    [FormerlySerializedAs("m_timeGain")]
    [SerializeField] private float m_fuelGain = 10.0f;
    [SerializeField] private bool useWebConfig = false;

    private void Awake()
    {
        if (useWebConfig)
            WebConfigHandler.OnFinishDownload(o => o.ExtractInt("fuel_gain", v => m_fuelGain = v));
    }


    public override void ExecutePowerUp(PlayerScriptContainer pHandler)
    {
        base.ExecutePowerUp(pHandler);
        if (pHandler.GetTimer != null) 
            pHandler.GetTimer.IncreaseTimeLeft(m_fuelGain);
    }
}
