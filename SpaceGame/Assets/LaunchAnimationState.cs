using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchAnimationState : StateWithView<BasicView>
{
    [SerializeField] private Animator launchAnimator;
    [SerializeField] private float CutsceneDuration = 10.0f;
    public override void EnterState()
    {
        base.EnterState();
        StartCoroutine(AdvanceFSM());
        launchAnimator.SetTrigger("animationTrigger");
    }

    IEnumerator AdvanceFSM()
    {
        Debug.Log($"Entered Coroutine to end launch animation in {CutsceneDuration} seconds");
        yield return new WaitForSeconds(CutsceneDuration);
        SelectView();
    }
    private void SelectView()
    {
        fsm.ChangeState<TutorialState>();
    }
    public override void ExitState()
    {
        launchAnimator?.ResetTrigger("animationTrigger");
        base.ExitState();

    }
}
