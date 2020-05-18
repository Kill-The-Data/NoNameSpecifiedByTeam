using UnityEngine;

public class IngameState : StateWithView<IngameView>
{
    public override void EnterState()
    {
        base.EnterState();
        InitGameState();
    }

    public void TimeOut()
    {
        fsm.ChangeState<MainMenuState>();
    }
    private void InitGameState()
    {
        //init time out timer
        view.GetTimeOutTimer().InitTimer(this);

        //init ingame timer
        TimerView timerView = view.GetTimer();
        timerView.InitTimer();

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
        timerView.gameObject.GetComponent<Timer>()?.Attach(deathWatch);
    }
    public void PlayerDied()
    {
        fsm.ChangeState<GameOverState>();
    }
    public void GameFinished()
    {
        //TO DO: ACTUALLY DO SOMETHING / CHANGE STATE
        Debug.Log("Finished game");
    }
}
