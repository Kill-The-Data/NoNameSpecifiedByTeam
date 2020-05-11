using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameState : StateWithView<IngameView>
{
    public override void EnterState()
    {
        base.EnterState();
        //init time out
        view.GetTimeOutTimer().InitTimer();
    }
    public override void ExitState()
    {
        base.ExitState();
        ResetScene();

    }

    public void TimOut()
    {
        base.ExitState();
        fsm.ChangeState<MainMenuState>();
    }
    private void ResetScene()
    {
        //TO DO : RESET SCENE
        //Reset player
        //reset debris
        //reset power ups if implemented?
        //reset score
    }
}
