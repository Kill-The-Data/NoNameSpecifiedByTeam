using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPowerUp : SoundPowerUp
{
    public int HealAmount= 0;

    public HealPowerUp(int healAmount = 20)
    {
        HealAmount = healAmount;
    }

    public override void ExecutePowerUp(PlayerScriptContainer pHandler)
    {
        base.ExecutePowerUp(pHandler);
        pHandler.GetPlayerHealth.Heal(HealAmount);
    }
}

