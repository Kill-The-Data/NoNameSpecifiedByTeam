using System.Collections.Generic;
using UnityEngine;
public class GoalReachedHandler : MonoBehaviour, IObserver
{

    private FSM m_FSM = null;
    private bool m_animationTriggered = false;
    [SerializeField] private List<Animator> m_AnimationObjects = new List<Animator>();
    [SerializeField] private float m_cutsceneDuration = 2.0f;
    [SerializeField] private CinematicController m_cinema;
    [SerializeField] private bool EndGameAfterTimeOut = false;
    private void Start()
    {
        m_FSM = GameObject.FindGameObjectWithTag("FSM")?.GetComponent<FSM>();
        Reset();
    }
    private void Reset()
    {
        //reset bool & animation trigger
        m_animationTriggered = false;
        foreach (Animator a in m_AnimationObjects)
        {
            a.gameObject.SetActive(false);
            a.ResetTrigger("AnimationTrigger");
        }
        //remove timer if attached
        if (GetComponent<Timer>())
            Destroy(gameObject.GetComponent<Timer>());
    }
    public void TriggerAnimation()
    {
        //activate all animations
        if (!m_animationTriggered)
            foreach (Animator a in m_AnimationObjects)
            {
                a.gameObject.SetActive(true);
                a.SetTrigger("AnimationTrigger");
            }
        m_animationTriggered = true;
        //set up timer 
        var timer = gameObject.AddComponent<Timer>();
        timer.Attach(this);
        timer.StartTimer(m_cutsceneDuration);
        m_cinema.LerpIn();
    }

    public void OnGoalReached()
    {
        //activate all animations
        if (!m_animationTriggered)
            foreach (Animator a in m_AnimationObjects)
            {
                a.gameObject.SetActive(true);
                a.SetTrigger("AnimationTrigger");
            }
        m_animationTriggered = true;
        //set up timer 
        var timer = gameObject.AddComponent<Timer>();
        timer.Attach(this);
        timer.StartTimer(m_cutsceneDuration);
        m_cinema.LerpIn();

    }

    public void FinishedGameCutScene()
    {
        //reset script && change to game finished state
        Reset();
        m_cinema.Reset(true);
        if(EndGameAfterTimeOut)
            EndGameState();
       
    }
    private void EndGameState()
    {
        if (m_FSM)
        {
            State currentState = m_FSM.GetCurrentState();
            if (currentState is IngameState ingameState)
                ingameState.GameFinished();
        }
    }

    public void GetUpdate(ISubject subject)
    {
        if (subject is Timer timer)
            if (timer.IsFinished())
                FinishedGameCutScene();
    }
}
