using UnityEngine;

public class TutorialState : StateWithView<IngameView>
{
    [SerializeField] private TutorialFSM m_tutorialFSM = null;
    [SerializeField] private AbstractView m_TutorialFSMView = null;
    [SerializeField] private int m_index = 2;
    private int m_currentIndex = 0;
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
        m_TutorialFSMView.EnableView();
        m_tutorialFSM.InitTutorial();
    }
    public void TrashCollected()
    {
    }
    public void TimeOut()
    {
        ExitState();
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

        var playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.ResetPlayerHealth();
        player.GetComponent<PlayerController>().ResetController();
        player.GetComponent<PlayerCargo>().ResetCargo();

        //configure Death Watch
        var deathWatch = player.GetComponent<DeathWatch>();
        if (!deathWatch)
            deathWatch = player.AddComponent<DeathWatch>();

        deathWatch.PlayerHealth = playerHealth;
        deathWatch.State = this;
    }
    public void PlayerDied()
    {
        InitPlayer();
    }
    public void FinishTutorial()
    {
        EventHandler.Instance.StationFilled -= StationFilled;
        m_TutorialFSMView.DisableView();
        fsm.ChangeState<IngameState>();
    }
    public void StationFilled()
    {
        m_currentIndex++;
        if (m_currentIndex == m_index)
            FinishTutorial();
    }
}
