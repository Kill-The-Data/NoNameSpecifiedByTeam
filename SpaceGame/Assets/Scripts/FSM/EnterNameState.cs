using System;
using UnityEngine;

public class EnterNameState : StateWithView<EnterNameView>
{
    public void OnNameEnteredComplete()
    {
        int score = PlayerPrefs.GetInt("score");
        string name = view.GetText();
        
        ReadWriteLeaderBoard.WriteScore(name,score,PlayerPrefs.GetString("hs_daily"));
        ReadWriteLeaderBoard.WriteScore(name,score,PlayerPrefs.GetString("hs_alltime"));
        
        fsm.ChangeState<HighscoreState>();
    }

    public void Update()
    {
        if (view.GetText().Length < 2 || view.GetText().Length > 15)
        {
            view.DisableButton();
        }
        else
        {
            view.EnableButton();
        }
    }
}
