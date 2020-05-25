using UnityEngine;

public class MainMenuState : StateWithView<BasicView>
{
    public void EnterGameState()
    {
        fsm.ChangeState<TutorialState>();
    }

    public override void EnterState()
    {

        Camera.main.GetComponent<camBlurScript>()?.ActivateBlur();
        base.EnterState();
    }

    public override void ExitState()
    {
        Camera.main.GetComponent<camBlurScript>()?.DeactivateBlur();
        base.ExitState();
    }

    public void EnterHighScoreState()
    {
        PlayerPrefs.SetInt("skip_feedback_state", 1);
        fsm.ChangeState<HighscoreState>();
    }


}
