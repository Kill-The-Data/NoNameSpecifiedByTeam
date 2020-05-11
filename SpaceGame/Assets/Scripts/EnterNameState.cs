using UnityEngine;

public class EnterNameState : StateWithView<EnterNameView>
{
    public void onNameEnteredComplete()
    {
        int score = PlayerPrefs.GetInt("score");
        string name = view.GetText();
        
        ReadWriteLeaderBoard.WriteScore(name,score,PlayerPrefs.GetString("hs_daily"));
        ReadWriteLeaderBoard.WriteScore(name,score,PlayerPrefs.GetString("hs_alltime"));
        
        fsm.ChangeState<HighscoreState>();
    }
    
    
}
