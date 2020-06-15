using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GoalReachedHandler : MonoBehaviour, IObserver
{

    private FSM m_FSM = null;
    private bool m_animationTriggered = false;
    [SerializeField] private List<Animator> m_AnimationObjects = new List<Animator>();
    [SerializeField] private float m_cutsceneDuration = 2.0f;
    [SerializeField] private CinematicController m_cinema;
    [FormerlySerializedAs("EndGameAfterTimeOut")] 
    [SerializeField] private bool m_endGameAfterTimeOut;
    
    //Setup
    private void Start()
    {
        m_FSM = GameObject.FindGameObjectWithTag("FSM")?.GetComponent<FSM>();
        Reset();
    }
    private void Reset()
    {
        //reset bool & animation trigger
        SetAnimation(false);
        //remove timer if attached
        if (this.GetComponentSafe(out Timer timer))
            Destroy(timer);
    }
    
    
    //triggers cutscene animation
    public void TriggerAnimation()
    {
        //activate all animations
        if (!m_animationTriggered)
            SetAnimation(true);
        BeginGameCutScene();
    }

    //alias
    public void OnGoalReached() => TriggerAnimation();

    //sets the animation active or inactive
    private void SetAnimation(bool active)
    {
        m_animationTriggered = active;
        foreach (Animator a in m_AnimationObjects)
        {
            a.gameObject.SetActive(active);
            if(active)
                a.SetTrigger("AnimationTrigger");
            else
                a.ResetTrigger("AnimationTrigger");
        }
    }
    
    //advances fsm
    private void EndGameState()
    {
        if (m_FSM)
        {
            State currentState = m_FSM.GetCurrentState();
            if (currentState is IngameState ingameState)
                ingameState.GameFinished();
        }
    }
    
    //sets up timer for cutscene and starts cutscene
    private void BeginGameCutScene()
    {
        //set up timer 
        var timer = gameObject.AddComponent<Timer>();
        timer.Attach(this);
        timer.StartTimer(m_cutsceneDuration);
        m_cinema.LowerBars();
    } 
    
    //deletes timer for cutscene, raises black-bars and finishes game-state
    private void FinishGameCutScene()
    {
        //reset script && change to game finished state
        Reset();
        m_cinema.Reset(true);
        if(m_endGameAfterTimeOut)
            EndGameState();
    }
    
    //listens for timer
    public void GetUpdate(ISubject subject)
    {
        if (subject is Timer timer)
            if (timer.IsFinished())
                FinishGameCutScene();
    }
}
