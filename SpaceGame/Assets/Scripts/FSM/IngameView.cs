using UnityEngine;

[RequireComponent(typeof(TimeOutTimer))]
public class IngameView : AbstractView
{

    [SerializeField] private TimeOutTimer m_TimeOut = null;
    public TimeOutTimer GetTimeOutTimer() => m_TimeOut;

    [SerializeField] private TimerView m_Timer = null;
    public TimerView GetTimer() => m_Timer;

    [SerializeField] private GameObject m_Player = null;
    public GameObject GetPlayer() => m_Player;
    
    private PerformanceMeasure m_Performance = null;
    public PerformanceMeasure GetPerformance() => m_Performance;
    private ItemSpawner m_Ispawner = null;
    public ItemSpawner GetItemSpawner() => m_Ispawner;

    [SerializeField] private VoronoiDebrisGen m_generator;
    [SerializeField] private bool m_genLevelInTutorial = false;

    [SerializeField] private GoalReachedHandler m_animation = null;
    public GoalReachedHandler GetAnimator() => m_animation;
    void Awake()
    {
        m_Performance = GetComponent<PerformanceMeasure>();
        m_Ispawner = GetComponent<ItemSpawner>();
    }

    public VoronoiDebrisGen GetLevelAlways() => m_generator;

    public VoronoiDebrisGen GetLevelIfGenerator(StateWithView<IngameView> state)
    {
        if ((state is TutorialState && m_genLevelInTutorial) || (state is IngameState && !m_genLevelInTutorial))
            return m_generator;
        return null;
    }
}
