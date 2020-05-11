using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreState : StateWithView<HighscoreView>
{
    [SerializeField] private GameObject m_entryPrefab;
    
    public override void EnterState()
    {
        base.EnterState();

        var weeklyPath = PlayerPrefs.GetString("hs_daily");
        var weeklyScore = ReadWriteLeaderBoard.ReadScores(weeklyPath);
        
        var tf = view.GetHighscoreListView().transform;
        
        foreach( var(player,score) in weeklyScore)
        {
            var entry = Instantiate(m_entryPrefab, tf).GetComponent<HighScoreListSetup>();
            entry.SetPlayer(player);
            entry.SetScore(score);
        }
    }

    public void OnBackToMainMenu()
    {
        fsm.ChangeState<MainMenuState>();
    }
    
}
