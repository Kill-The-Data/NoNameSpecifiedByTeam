using UnityEngine;

public class HighscoreState : StateWithView<HighscoreView>
{
    [SerializeField] private GameObject m_entryPrefab;
    
    public override void EnterState()
    {
        base.EnterState();

        var weeklyPath = PlayerPrefs.GetString("hs_daily");
        var weeklyScore = ReadWriteLeaderBoard.ReadScores(weeklyPath);
        {
            var tf = view.GetHighscoreListView().transform;
            
            //the first child is the heading so we skip it
            for (int i = 1; i < tf.childCount; ++i)
            {
                Destroy(tf.GetChild(i).gameObject);
            }
            
            foreach( var(player,score) in weeklyScore)
            {
                var entry = Instantiate(m_entryPrefab, tf).GetComponent<HighScoreListSetup>();
                entry.SetPlayer(player);
                entry.SetScore(score);
            }
        }
        var alltimePath = PlayerPrefs.GetString("hs_alltime");
        var allTimeScore = ReadWriteLeaderBoard.ReadScores(alltimePath);
        {
            var tf = view.GetHighscoreListViewAllTime().transform;
            
            //the first child is the heading so we skip it
            for (int i = 1; i < tf.childCount; ++i)
            {
                Destroy(tf.GetChild(i).gameObject);
            }
            
            foreach( var(player,score) in allTimeScore)
            {
                var entry = Instantiate(m_entryPrefab, tf).GetComponent<HighScoreListSetup>();
                entry.SetPlayer(player);
                entry.SetScore(score);
            }
        }
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
