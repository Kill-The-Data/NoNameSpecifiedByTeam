using UnityEngine;

public class HighscoreView : AbstractView
{
    private HIghScoreDisplay m_highscoreDisplay;
    public HIghScoreDisplay GetHIghScoreDisplay() => m_highscoreDisplay;


    void Awake()
    {
        m_highscoreDisplay = GetComponent<HIghScoreDisplay>();
    }
}
