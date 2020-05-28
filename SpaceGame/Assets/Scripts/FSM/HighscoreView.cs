using UnityEngine;

public class HighscoreView : AbstractView
{
    private HighScoreDisplay m_highscoreDisplay;
    public HighScoreDisplay GetHighScoreDisplay() => m_highscoreDisplay;


    void Awake()
    {
        FindComponents();
    }

    private void FindComponents()
    {
        m_highscoreDisplay = GetComponent<HighScoreDisplay>();
    }
    public void LoadComponents()
    {
        FindComponents();
    }
}
