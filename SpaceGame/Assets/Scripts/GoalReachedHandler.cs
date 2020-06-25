using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using EventHandler = SpaceGame.EventHandler;

public class GoalReachedHandler : MonoBehaviour, IObserver
{

    private FSM m_FSM = null;
    private bool m_animationTriggered = false;
    [SerializeField] private List<Animator> m_AnimationObjects = new List<Animator>();
    [SerializeField] private float m_cutsceneDuration = 2.0f;
    [SerializeField] private CinematicController m_cinema;
    [FormerlySerializedAs("EndGameAfterTimeOut")]
    [SerializeField] private bool m_endGameAfterTimeOut;
    [SerializeField] private bool m_SetActiveOnStart = false;
    [SerializeField] private float m_CameraRideDuration = 10.0f;
    [SerializeField] private bool m_TriggerCameraRide = false;
    private bool m_finishedCameraRide = false;
    [SerializeField] private CamWayPointNavigator m_CamNavigator = null;
    //Setup
    private void OnEnable()
    {
        m_FSM = GameObject.FindGameObjectWithTag("FSM")?.GetComponent<FSM>();
        Reset();
    }
    private void Reset()
    {
        if (m_CamNavigator)
            m_CamNavigator.EndRide();
        m_finishedCameraRide = false;
        //reset bool & animation trigger
        SetAnimation(m_SetActiveOnStart);
        //remove timer if attached
        if (this.GetComponentSafe(out Timer timer))
            Destroy(timer);

        EventHandler.Instance.EndCutscene();

    }


    //triggers cutscene animation
    public void TriggerAnimation()
    {
        EventHandler.Instance.StartCutscene();
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
            if (active)
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
    private void StartCameraRide()
    {
        m_finishedCameraRide = true;
        //start ride
        if (m_CamNavigator)
            m_CamNavigator.StartRide();
        //restart timer for camera ride
        GetComponent<Timer>()?.StartTimer(m_CameraRideDuration);
    }
    //deletes timer for cutscene, raises black-bars and finishes game-state
    private void FinishGameCutScene()
    {
        //if camera ride should be triggered, check if it was already triggered & play it
        if (m_TriggerCameraRide)
        {
            if (!m_finishedCameraRide)
            {
                Debug.Log("starting camera");
                StartCameraRide();
                return;
            }
        }
        //reset script && change to game finished state
        Reset();
        m_cinema.Reset(true);
        if (m_endGameAfterTimeOut)
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
