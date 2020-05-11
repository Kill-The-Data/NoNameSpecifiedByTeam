using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameState : StateWithView<IngameView>
{
    public override void EnterState()
    {
        base.EnterState();
        InitGameState();
    }
    public override void ExitState()
    {
        base.ExitState();
    }

    public void TimOut()
    {
        base.ExitState();
        fsm.ChangeState<MainMenuState>();
    }
    private void InitGameState()
    {
        //init time out timer
        view.GetTimeOutTimer().InitTimer();

        //init ingame timer
        view.GetTimer().InitTimer();

        //reset cargo & player pos
        GameObject player = view.GetPlayer();
        player.GetComponent<PlayerController>().ResetController();
        player.GetComponent<PlayerCargo>().ResetCargo();

        //reset score
        view.GetScore().Reset();

    }
   
}
