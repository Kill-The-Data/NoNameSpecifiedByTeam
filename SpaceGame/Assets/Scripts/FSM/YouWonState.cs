
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouWonState : StateWithView<BasicView>
{

    public int Timeout = 5;

    public override void EnterState()
    {
        base.EnterState();
        StartCoroutine(AdvanceFSM());
    }

    IEnumerator AdvanceFSM()
    {
        Debug.Log($"Entered Coroutine to end GameOverScreen in {Timeout} seconds");
        yield return new WaitForSeconds(Timeout);
        SelectView();
    }

    public void SelectView()
    {
        fsm.ChangeState<GameResultState>();
    }
}
