using UnityEngine;

public class MainMenuState : StateWithView<HighscoreView>
{
    public void EnterGameState()
    {
        fsm.ChangeState<TutorialState>();
    }

    public override void EnterState()
    {
        view.LoadComponents();
        view.GetHighScoreDisplay()?.Load();
        ResetPlayerPrefs();
        Camera.main.GetComponent<CamBlurScript>()?.ActivateBlur();
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public void EnterHighScoreState()
    {
        PlayerPrefs.SetInt("skip_feedback_state", 1);
        fsm.ChangeState<HighscoreState>();
    }

    public void EnterCreditsState()
    {
        fsm.ChangeState<CreditsState>();
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
