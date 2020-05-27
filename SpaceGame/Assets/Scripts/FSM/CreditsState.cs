using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsState : StateWithView<BasicView>
{
    public void EnterMainMenu()
    {
        fsm.ChangeState<MainMenuState>();
    }
}
