using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreListSetup : MonoBehaviour
{
    [SerializeField] private TMP_Text m_player;
    [SerializeField] private TMP_Text m_score;


    public void SetPlayer(string s)
    {
        m_player.text = s;
    }

    public void SetScore(int s)
    {
        m_score.text = s.ToString();
    }
    
}
