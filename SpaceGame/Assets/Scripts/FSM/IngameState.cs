using UnityEngine;

public class IngameState : StateWithView<IngameView>
{
    public override void EnterState()
    {
        Debug.Log("entering ingame state");
        base.EnterState();
        InitGameState();
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

        //init ingame timer
        TimerView timerView = view.GetTimer();
        timerView.InitTimer();
        //only returns timer if timer view has been initialized
        Timer timer = timerView.gameObject.GetComponent<Timer>();

        //reset cargo & player pos
        GameObject player = view.GetPlayer();
        // player.GetComponent<PlayerController>().ResetController();
        player.GetComponent<PlayerCargo>().ResetCargo();

        var playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.ResetPlayerHealth();

        //configure Death Watch
        var deathWatch = player.GetComponent<DeathWatch>();
        if (!deathWatch)
            deathWatch = player.AddComponent<DeathWatch>();

        deathWatch.PlayerHealth = playerHealth;
        deathWatch.State = this;
        timer.Attach(deathWatch);

        //reset score
        view.GetScore().Reset();

    }

    public void PlayerDied()
    {
        ExitState();
        fsm.ChangeState<GameOverState>();
    }
}
