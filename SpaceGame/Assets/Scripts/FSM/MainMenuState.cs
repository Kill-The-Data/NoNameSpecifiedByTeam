using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : StateWithView<BasicView>
{

    public void EnterGameState()
    {
        ExitState();
        fsm.ChangeState<IngameState>();
    }
}
