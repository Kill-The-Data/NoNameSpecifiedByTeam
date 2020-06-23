using System;
using UnityEngine;

public class Mail : MonoBehaviour
{
    private Action OnPickedUp;
    
    void Start()
    {
        MailCounter.OnInstance(instance =>
        {
            OnPickedUp += instance.AddMail;
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player-Collector"))
        {
            OnPickedUp?.Invoke();
            Destroy(gameObject);
        }
    }
}
