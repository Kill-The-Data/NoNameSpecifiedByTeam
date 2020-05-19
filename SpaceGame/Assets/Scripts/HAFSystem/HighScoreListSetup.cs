using TMPro;
using UnityEngine;

public class HighScoreListSetup : MonoBehaviour
{
    [Tooltip("The text field to display the player's name in")]
    [SerializeField] private TMP_Text m_player;
    [Tooltip("The text field to display the player's score in")]
    [SerializeField] private TMP_Text m_score;

    //for the next sections @see HighscoreState
    
    //sets the player name
    public void SetPlayer(string s)
    {
        m_player.text = s;
    }

    //sets the player score
    public void SetScore(int s)
    {
        m_score.text = s.ToString();
    }
    
}
