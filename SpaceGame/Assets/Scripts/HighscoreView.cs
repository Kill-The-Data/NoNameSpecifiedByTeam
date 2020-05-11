using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreView : AbstractView
{
    [SerializeField] private GameObject m_highscoreListView;
    public GameObject GetHighscoreListView() => m_highscoreListView;

}
