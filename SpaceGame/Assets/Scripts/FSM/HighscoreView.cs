using UnityEngine;

public class HighscoreView : AbstractView
{
    [SerializeField] private GameObject m_highscoreListView;
    public GameObject GetHighscoreListView() => m_highscoreListView;
    
    [SerializeField] private GameObject m_highscoreListViewAllTime;
    public GameObject GetHighscoreListViewAllTime() => m_highscoreListViewAllTime;

}
