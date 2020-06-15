using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

//singleton that stores event handler to grand global access to it
public sealed class EventSingleton
{
    public EventHandler EventHandler = null;

    [Obsolete("use EventHandler.Instance instead")]
    public static EventSingleton Instance { get; } = new EventSingleton();
}

public class EventHandler : MonoBehaviour
{
    public static EventHandler Instance;
    
    //events
    public event Action StationFilled;
    public event Action GameStart;
    public event Action GameFinished;
    public event Action TutorialStart;


    void Awake()
    {
        InitSingleton();
        Init();
    }
    //set handler for singleton
    private void InitSingleton()
    {
        Instance = this;
        EventSingleton.Instance.EventHandler = this;
    }
    private void Init()
    {
        StationFilled += OnStationFilled;
    }

    public void FinishGame()
    {
        GameFinished?.Invoke();
    }
    public void NewStationFilled()
    {
        StationFilled?.Invoke();
    }
    //increase player pref on event call
    public void OnStationFilled()
    {
        PlayerPrefs.SetInt(("buoysFilled"), (PlayerPrefs.GetInt("buoysFilled") + 1));
    }

    public void StartTutorial()
    {
        TutorialStart?.Invoke();
    }
    
    public void StartGame()
    {
        GameStart?.Invoke();
    }
    //test input did not use space this time around
    public void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Equals))
            StationFilled?.Invoke();
        #endif
    }

}


