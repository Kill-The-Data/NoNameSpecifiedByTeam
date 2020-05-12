using System.Collections;
using System.Collections.Generic;
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
        view.GetTimeOutTimer().InitTimer();

        InitPlayer();
    }
    private void InitPlayer()
    {
        //reset cargo & player pos
        GameObject player = view.GetPlayer();
        player.GetComponent<PlayerController>().ResetController();
        player.GetComponent<PlayerCargo>().ResetCargo();

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
        ExitState();
        fsm.ChangeState<IngameState>();

    }
}
