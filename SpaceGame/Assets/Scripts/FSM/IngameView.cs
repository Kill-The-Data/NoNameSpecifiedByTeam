using UnityEngine;

public class IngameView : AbstractView
{

    [SerializeField] private TimeOutTimer m_TimeOut = null;
    public TimeOutTimer GetTimeOutTimer() => m_TimeOut;

    [SerializeField] private TimerView m_Timer = null;
    public TimerView GetTimer() => m_Timer;

    [SerializeField] private GameObject m_Player = null;
    public GameObject GetPlayer() => m_Player;

    [SerializeField] private ScoreUI m_Score = null;
    public ScoreUI GetScore() => m_Score;
    private PerformanceMeasure m_Performance = null;
    public PerformanceMeasure GetPerformance() => m_Performance;
    private ItemSpawner m_Ispawner = null;
    public ItemSpawner GetItemSpawner() => m_Ispawner;
    void Awake()
    {
        m_Performance = GetComponent<PerformanceMeasure>();
        m_Ispawner = GetComponent<ItemSpawner>();
    }
}
