using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPowerUp : PowerUp
{
    public int HealAmount= 0;

    public HealPowerUp(int healAmount = 20)
    {
        HealAmount = healAmount;
    }

    public override void ExecutePowerUp(PlayerScriptContainer pHandler)
    {
        pHandler.GetPlayerHealth.Heal(HealAmount);
    }
}

