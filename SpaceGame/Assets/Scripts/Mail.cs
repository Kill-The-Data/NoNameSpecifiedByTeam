using System;
using UnityEngine;

public class Mail : AbstractCollider
{
    private Action OnPickedUp;
    
    void Start()
    {
        MailCounter.OnInstance(instance =>
        {
            OnPickedUp += instance.AddMail;
        });
    }

    protected override void HandlePlayerEnter(Collider other)
    {
        OnPickedUp?.Invoke();
        Destroy(gameObject);
    }
}
