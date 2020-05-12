using System.Collections;
using UnityEngine;

public class GameOverState : StateWithView<BasicView>
{
    public int Timeout = 5;
    
    public override void EnterState()
    {
        base.EnterState();
        StartCoroutine(AdvanceFSM());
    }

    IEnumerator AdvanceFSM()
    {
        Debug.Log($"Entered Corroutine to end GameOverScreen in {Timeout} seconds");
        yield return new WaitForSeconds(Timeout);
        SelectView();
    }

    public void SelectView()
    {
        int score = PlayerPrefs.GetInt("score");
        if (ReadWriteLeaderBoard.IsOnLeaderboard(score, PlayerPrefs.GetString("hs_daily")) ||
            ReadWriteLeaderBoard.IsOnLeaderboard(score, PlayerPrefs.GetString("hs_alltime")))
        {
            fsm.ChangeState<EnterNameState>();
        }
        else
        {
            fsm.ChangeState<MainMenuState>();
        }
    }
}
