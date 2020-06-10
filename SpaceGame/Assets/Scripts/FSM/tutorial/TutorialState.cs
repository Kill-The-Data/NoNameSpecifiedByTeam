using UnityEngine;

public class TutorialState : StateWithView<IngameView>
{

    [SerializeField] private TutorialFSM m_tutorialFSM = null;
    [SerializeField] private AbstractView m_TutorialFSMView = null;
    //init tutorial fsm on state enter 
    public override void EnterState()
    {
        base.EnterState();
        InitGameState();
        m_TutorialFSMView.EnableView();
        m_tutorialFSM.InitTutorial();
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

        //reset everything that is tagged with reset and has a reset script attached
        GameObject[] resetObjs = GameObject.FindGameObjectsWithTag("Reset");
        foreach (GameObject currentObj in resetObjs)
        {
            currentObj.GetComponent<AReset>()?.Reset();
        }
        
        EventSingleton.Instance.EventHandler.StartTutorial();
    }
    private void InitPlayer()
    {
        //reset cargo & player pos
        GameObject player = view.GetPlayer();
        player.GetComponent<PlayerController>().ResetController();
        player.GetComponent<PlayerCargo>().ResetCargo();
        player.GetComponentInChildren<PlayerCargoVisual>().Reset();
        view.GetScore().Reset();

        var playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.ResetPlayerHealth();

        //configure Death Watch
        var deathWatch = player.GetComponent<DeathWatch>();
        if (!deathWatch)
            deathWatch = player.AddComponent<DeathWatch>();

        deathWatch.PlayerHealth = playerHealth;
        deathWatch.State = this;
    }
    public void PlayerDied()
    {
        Debug.Log("Reseting player");
        InitPlayer();
    }
    public void FinishTutorial()
    {
        m_TutorialFSMView.DisableView();
        fsm.ChangeState<IngameState>();
    }

}
