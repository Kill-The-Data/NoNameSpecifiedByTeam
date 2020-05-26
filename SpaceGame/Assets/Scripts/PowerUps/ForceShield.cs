using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceShield : SoundPowerUp
{
    public override void ExecutePowerUp(PlayerScriptContainer pHandler)
    {
        base.ExecutePowerUp(pHandler);
        pHandler.GetShieldState.ActivateShield();
    }
}
