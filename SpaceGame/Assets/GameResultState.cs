using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResultState : StateWithView<ScoreCalcView>
{
    [Header("--Please make sure this is longer than the duration of score calculation--")]
    public int Timeout = 8;

    public override void EnterState()
    {
        base.EnterState();
        LerpScore  scoreCalc =view.GetScoreCalc();
        scoreCalc.Reset();
        scoreCalc.CalculateScore(100, PlayerPrefs.GetFloat("time"),
                PlayerPrefs.GetInt("health"), 0, PlayerPrefs.GetInt("goalReached"));
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
        fsm.ChangeState<GameResultState>();
        int score = PlayerPrefs.GetInt("score");
        if (ReadWriteLeaderBoard.IsOnLeaderboard(score, PlayerPrefs.GetString("hs_daily")) ||
            ReadWriteLeaderBoard.IsOnLeaderboard(score, PlayerPrefs.GetString("hs_alltime")))
        {
            fsm.ChangeState<EnterNameState>();
        }
        else
        {
            fsm.ChangeState<FeedbackState>();
        }
    }
}
