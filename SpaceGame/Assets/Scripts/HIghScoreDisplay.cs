using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HIghScoreDisplay : MonoBehaviour
{
    [SerializeField] private GameObject m_prefab;
    [SerializeField] private Transform m_dailyTransform;
    [SerializeField] private Transform m_allTimeTransform;


    public void Load()
    {
        var weeklyPath = PlayerPrefs.GetString("hs_daily");
        var weeklyScore = ReadWriteLeaderBoard.ReadScores(weeklyPath);
        {

            //the first child is the heading so we skip it
            for (int i = 1; i < m_dailyTransform.childCount; ++i)
            {
                Destroy(m_dailyTransform.GetChild(i).gameObject);
            }

            foreach (var (player, score) in weeklyScore)
            {
                var entry = Instantiate(m_prefab, m_dailyTransform).GetComponent<HighScoreListSetup>();
                entry.SetPlayer(player);
                entry.SetScore(score);
            }
        }
        var alltimePath = PlayerPrefs.GetString("hs_alltime");
        var allTimeScore = ReadWriteLeaderBoard.ReadScores(alltimePath);
        {

            //the first child is the heading so we skip it
            for (int i = 1; i < m_allTimeTransform.childCount; ++i)
            {
                Destroy(m_allTimeTransform.GetChild(i).gameObject);
            }

            foreach (var (player, score) in allTimeScore)
            {
                var entry = Instantiate(m_prefab, m_allTimeTransform).GetComponent<HighScoreListSetup>();
                entry.SetPlayer(player);
                entry.SetScore(score);
            }
        }
    }
}
