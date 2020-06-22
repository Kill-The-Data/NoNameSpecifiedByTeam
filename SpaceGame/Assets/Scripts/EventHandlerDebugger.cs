using System;
using UnityEngine;
using EventHandler = SpaceGame.EventHandler;
public class EventHandlerDebugger : MonoBehaviour
{
    void Start()
    {
        var eh = EventHandler.Instance;
        
        eh.TutorialStart += GenDebug("Tutorial Start");
        eh.GameStart += GenDebug("Game Start");
        eh.GameFinished += GenDebug("Game Finished");
    }

    private Action GenDebug(string s)
    {
        return delegate { Debug.Log("[Event Handler]" + s); };
    }
    
}
