using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceShield : PowerUp
{

    public override void ExecutePowerUp(PlayerScriptContainer pHandler)
    {
        pHandler.GetShieldState.ActivateShield();
    }
}
