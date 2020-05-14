using UnityEngine;

public class MainMenuState : StateWithView<BasicView>
{
    public void EnterGameState()
    {
        fsm.ChangeState<TutorialState>();
    }

    public void EnterHighScoreState()
    {
        PlayerPrefs.SetInt("skip_feedback_state", 1);
        fsm.ChangeState<HighscoreState>();
    }


}
