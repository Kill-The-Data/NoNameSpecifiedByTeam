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
        InitPlayer();

        //reset everything that is tagged with reset and has a reset script attached
        GameObject[] resetObjs = GameObject.FindGameObjectsWithTag("Reset");
        foreach (GameObject currentObj in resetObjs)
        {
            currentObj.GetComponent<AReset>()?.Reset();
        }

    }
    private void InitPlayer()
    {
        //reset cargo & player pos
        GameObject player = view.GetPlayer();
        player.GetComponent<PlayerController>().ResetController();
        player.GetComponent<PlayerCargo>().ResetCargo();
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
