using System;
using UnityEngine;
using EventHandler = SpaceGame.EventHandler;

public class Mail : AbstractCollider
{
    private Action OnPickedUp;
    
    void Start()
    {
        MailCounter.OnInstance(instance =>
        {
            OnPickedUp += instance.AddMail;
        });
        EventHandler.Instance.TutorialStart += Reset;
    }
    
    
    
    void Reset()
    {
        gameObject.SetActive(true);
    }

    protected override void HandlePlayerEnter(Collider other)
    {
        if(gameObject.activeSelf)
            OnPickedUp?.Invoke();
        gameObject.SetActive(false);
    }
}
