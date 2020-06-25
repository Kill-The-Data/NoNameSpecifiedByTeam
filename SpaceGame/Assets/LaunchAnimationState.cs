using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchAnimationState : StateWithView<BasicView>
{
    [SerializeField] private Animator m_launchAnimator;
    [SerializeField] private float m_CutsceneDuration = 10.0f;
    [SerializeField] private float m_Deltatransition = 0.2f;
    [SerializeField] private FadeController m_FadeController = null;
    public override void EnterState()
    {
        base.EnterState();
        StartCoroutine(AdvanceFSM());
        StartCoroutine(Transition());
        m_launchAnimator.SetTrigger("animationTrigger");
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
        base.ExitState();

    }
}
