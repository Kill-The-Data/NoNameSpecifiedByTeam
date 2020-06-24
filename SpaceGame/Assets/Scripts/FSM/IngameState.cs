using UnityEngine;
using EventHandler = SpaceGame.EventHandler;
public class IngameState : StateWithView<IngameView>
{

    public override void EnterState()
    {
        base.EnterState();
        InitGameState();
    }

    public override void ExitState()
    {
        base.ExitState();
        EventHandler.Instance.FinishGame();
    }

    public void TimeOut()
    {
        EventHandler.Instance.FinishGame();
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
        

        //init time out timer
        view.GetTimeOutTimer().InitTimer(this);

        //init item spawn
        view.GetItemSpawner().Reset();

       

        //reset cargo & player pos
        GameObject player = view.GetPlayer();
        // player.GetComponent<PlayerController>().ResetController();
        player.GetComponent<PlayerCargo>().ResetCargo();

        var playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.ResetPlayerHealth();
        //attach performance measure

        //configure Death Watch
        var deathWatch = player.GetComponent<DeathWatch>();
        if (!deathWatch)
            deathWatch = player.AddComponent<DeathWatch>();

        deathWatch.PlayerHealth = playerHealth;
        deathWatch.State = this;
      //  timerView.gameObject.GetComponent<Timer>()?.Attach(deathWatch);
        
        EventHandler.Instance.StartGame();
    }
    public void PlayerDied()
    {
        view.GetPerformance().StoreStatsInPlayerPrefs(0);
        view.GetItemSpawner().Deactivate();
        fsm.ChangeState<GameOverState>();

    }
    public void GameFinished()
    {
        view.GetPerformance().StoreStatsInPlayerPrefs(1);
        view.GetItemSpawner().Deactivate();
        fsm.ChangeState<YouWonState>();
    }
}
