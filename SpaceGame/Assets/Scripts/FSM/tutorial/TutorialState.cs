using UnityEngine;
using EventHandler = SpaceGame.EventHandler;
public class TutorialState : StateWithView<IngameView>
{
    [SerializeField] private TutorialFSM m_tutorialFSM = null;
    [SerializeField] private AbstractView m_TutorialFSMView = null;
    [SerializeField] private int m_index = 2;
    private int m_currentIndex = 0;
    private DoorController m_doorController = null;
    //init tutorial fsm on state enter 
    private void OnDestroy()
    {
        EventHandler.Instance.StationFilled -= StationFilled;
    }
    public override void EnterState()
    {
        EventHandler.Instance.StationFilled += StationFilled;
        m_currentIndex = 0;
        base.EnterState();
        InitGameState();
        InitTutorial();
        m_TutorialFSMView.EnableView();
        m_tutorialFSM.InitTutorial();
    }
    public override void ExitState()
    {
        EventHandler.Instance.StationFilled -= StationFilled;
        base.ExitState();
    }
    public void TimeOut()
    {
        EventHandler.Instance.FinishGame();
        fsm.ChangeState<MainMenuState>();
    }
    private void InitGameState()
    {
        //init time out timer
        view.GetTimeOutTimer().InitTimer(this);
        view.GetTimer().gameObject.SetActive(false);

        var levelGen = view.GetLevelAlways();


     

        //delete level and prewarm a new one
        levelGen.DeleteLevel();
        levelGen.Prewarm();

        //apply the prewarm only if the tutorial is supposed to 
        view.GetLevelIfGenerator(this)?.ApplyPrewarm();
        InitPlayer();

        EventHandler.Instance.StartTutorial();
    }
    private void InitPlayer()
    {
        //reset cargo & player pos
        GameObject player = view.GetPlayer();


        PerformanceMeasure playerPerformance = view.GetPerformance();
        //init ingame timer
        TimerView timerView = view.GetTimer();
        timerView.InitTimer();
        timerView.AttachPerformanceMeasure(playerPerformance);

        var playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.ResetPlayerHealth();
        player.GetComponent<PlayerController>().ResetController();
        //player.GetComponent<PlayerCargo>().ResetCargo();
        playerHealth.Attach(playerPerformance);

        //configure Death Watch
        var deathWatch = player.GetComponent<DeathWatch>();
        if (!deathWatch)
            deathWatch = player.AddComponent<DeathWatch>();

        deathWatch.PlayerHealth = playerHealth;
        deathWatch.State = this;
        timerView.gameObject.GetComponent<Timer>()?.Attach(deathWatch);
    }
    public void PlayerDied()
    {
        InitPlayer();
    }
    public void FinishTutorial()
    {
        m_doorController?.OpenSecondDoor();

        EventHandler.Instance.StationFilled -= StationFilled;
        m_TutorialFSMView.DisableView();
        fsm.ChangeState<IngameState>();
    }
    public void StationFilled()
    {
             m_currentIndex++;
        if (m_currentIndex == 1)
            m_doorController?.OpenFirstDoor();
        if (m_currentIndex == m_index)
            FinishTutorial();
    }

    private void InitTutorial()
    {
        if (m_doorController == null)
        {
            m_doorController = GetComponent<DoorController>();
        }
        m_doorController?.InitDoors();

    }
}

