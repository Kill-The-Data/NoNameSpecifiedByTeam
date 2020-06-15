using UnityEngine;

using System.Collections;
public class HighscoreState : StateWithView<HighscoreView>
{
    public int Timeout = 15;

    public override void EnterState()
    {
        base.EnterState();
        view.GetHighScoreDisplay()?.Load();
        StartCoroutine(AdvanceFSM());
    }
    IEnumerator AdvanceFSM()
    {
        Debug.Log($"Entered Coroutine to end GameOverScreen in {Timeout} seconds");
        yield return new WaitForSeconds(Timeout);
        OnBackToMainMenu();
    }

    public void OnBackToMainMenu()
    {
        if (PlayerPrefs.GetInt("skip_feedback_state", 0) == 1)
        {
            PlayerPrefs.SetInt("skip_feedback_state", 0);
            fsm.ChangeState<MainMenuState>();
            return;
        }
        fsm.ChangeState<FeedbackState>();
    }
    
}
