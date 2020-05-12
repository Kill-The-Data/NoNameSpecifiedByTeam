using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractTutorialState : StateWithView<AbstractView>
{
    //reference to tutorial state machine to avoid continius type casting
    protected TutorialFSM m_tFSM;
    //inits SM & casts it to  tutorial SM
    public override void Initialize(FSM pFsm)
    {
        if (pFsm is TutorialFSM newFSM)
        {
            m_tFSM = newFSM;
        }
       view?.DisableView();
    }
}
