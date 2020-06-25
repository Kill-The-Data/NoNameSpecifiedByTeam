using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchAnimationState : StateWithView<BasicView>
{
    [SerializeField] private Animator m_launchAnimator;
    [SerializeField] private Animator m_dollyAnimator;
    [SerializeField] private float m_CutsceneDuration = 10.0f;
    [SerializeField] private float m_Deltatransition = 0.2f;
    [SerializeField] private FadeController m_FadeController = null;
    [SerializeField] private Canvas c = null;
    private float awaitDuration = 4.0f;
    public override void EnterState()
    {
        c.enabled=true;
        base.EnterState();
        StartCoroutine(AwaitAnimation());
    }

    IEnumerator AwaitAnimation() 
    {
        yield return new WaitForSeconds(awaitDuration);
        TriggerAnimation();
    }
    IEnumerator AdvanceFSM()
    {
        Debug.Log($"Entered Coroutine to end launch animation in {m_CutsceneDuration} seconds");
        yield return new WaitForSeconds(m_CutsceneDuration);
        SelectView();
    }
    IEnumerator Transition()
    {
        yield return new WaitForSeconds(m_CutsceneDuration - m_Deltatransition);
        TriggerTransition();
    }
    private void TriggerTransition()
    {
        m_FadeController?.Fade();
    }
    private void SelectView()
    {
        fsm.ChangeState<TutorialState>();
    }
    public override void ExitState()
    {
        m_launchAnimator?.ResetTrigger("animationTrigger");
        m_dollyAnimator?.ResetTrigger("animationTrigger");

        base.ExitState();

    }
    public void TriggerAnimation()
    {
        c.enabled = false;

        StartCoroutine(AdvanceFSM());
        StartCoroutine(Transition());
        m_launchAnimator.SetTrigger("animationTrigger");
        m_dollyAnimator?.SetTrigger("animationTrigger");
    }
}
