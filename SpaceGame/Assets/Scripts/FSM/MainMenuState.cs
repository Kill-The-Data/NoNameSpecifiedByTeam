using UnityEngine;

public class MainMenuState : StateWithView<BasicView>
{
    public void EnterGameState()
    {
        fsm.ChangeState<TutorialState>();
    }

    public override void EnterState()
    {
        ResetPlayerPrefs();
        Camera.main.GetComponent<CamBlurScript>()?.ActivateBlur();
        base.EnterState();
    }

    public override void ExitState()
    {
        Camera.main.GetComponent<CamBlurScript>()?.DeactivateBlur();
        base.ExitState();
    }

    public void EnterHighScoreState()
    {
        PlayerPrefs.SetInt("skip_feedback_state", 1);
        fsm.ChangeState<HighscoreState>();
    }

    private void ResetPlayerPrefs()
    {
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetFloat("time", 0);
        PlayerPrefs.SetInt("health", 0);
        PlayerPrefs.SetInt("goalReached",0);
        PlayerPrefs.SetInt("buoysFilled",0);
    }

}
