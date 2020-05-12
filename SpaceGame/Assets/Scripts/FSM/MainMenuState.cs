using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : StateWithView<BasicView>
{
    [SerializeField] private bool m_SkipTutorial = false;
    public void EnterGameState()
    {
        ExitState();
        if (m_SkipTutorial) fsm.ChangeState<IngameState>();
        else if (!m_SkipTutorial) fsm.ChangeState<TutorialState>();
    }
}
