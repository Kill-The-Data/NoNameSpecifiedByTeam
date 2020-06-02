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

        var levelGen = view.GetLevelAlways();
        
        //check if the level was prewarmed by the tutorial, if it was not (because the tutorial was skipped, or something simmilar)
        //delete the previous data and make sure the prewarm the level now
        if(!levelGen.WasPrewarmed){
            levelGen.DeleteLevel();
            levelGen.Prewarm();
        }
        
        //apply the prewarm, only if the state is also the state where the prewarm should be applied
        view.GetLevelIfGenerator(this)?.ApplyPrewarm();
        
        PerformanceMeasure playerPerformance = view.GetPerformance();

        //init time out timer
        view.GetTimeOutTimer().InitTimer(this);

        //init item spawn
        view.GetItemSpawner().Reset();

        //init ingame timer
        TimerView timerView = view.GetTimer();
        timerView.InitTimer();
        timerView.AttachPerformanceMeasure(playerPerformance);

        //reset cargo & player pos
        GameObject player = view.GetPlayer();
        // player.GetComponent<PlayerController>().ResetController();
        player.GetComponent<PlayerCargo>().ResetCargo();

        var playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.ResetPlayerHealth();
        //attach performance measure
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
        view.GetPerformance().StoreStatsInPlayerPrefs(0);
        view.GetItemSpawner().Deactivate();
        Camera.main.GetComponent<CamBlurScript>().ActivateBlur();
        fsm.ChangeState<GameOverState>();

    }
    public void GameFinished()
    {
        view.GetPerformance().StoreStatsInPlayerPrefs(1);
        view.GetItemSpawner().Deactivate();
        Camera.main.GetComponent<CamBlurScript>().ActivateBlur();
        fsm.ChangeState<YouWonState>();
    }
}
