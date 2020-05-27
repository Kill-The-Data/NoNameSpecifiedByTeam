using UnityEngine;

public class HighscoreView : AbstractView
{
    private HighScoreDisplay m_highscoreDisplay;
    public HighScoreDisplay GetHighScoreDisplay() => m_highscoreDisplay;


    void Awake()
    {
        m_highscoreDisplay = GetComponent<HighScoreDisplay>();
    }
}
