using System;
using UnityEngine;
using EventHandler = SpaceGame.EventHandler;
public abstract class AReset : MonoBehaviour
{
    public void Start()
    {
        Debug.Log("[Abstract Reset] Reset");
        EventHandler.Instance.TutorialStart += Reset;
    }

    public void Remove()
    {
        EventHandler.Instance.TutorialStart -= Reset;
    }

    public void OnDestroy()
    {
       Remove();
    }

    public abstract void Reset();
}
